using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HSMY_AdminWeb.Models;
using JGCK.Util.CloudStorage;
using JGCK.Web.Admin.Models;
using JGCK.Web.General;
using JGCK.Web.General.VO;

namespace JGCK.Web.Admin.Controllers
{
    public class AjaxCommonController : JGCK_MvcController
    {
        private int FileSize => int.Parse(ConfigurationManager.AppSettings["uploadFileSize"] ?? "0");
        private string SaveFilePath => ConfigurationManager.AppSettings["saveUploadedFilePath"];
        private string VisitResUrl => ConfigurationManager.AppSettings["visitUploadedFileUrl"];

        public JsonResult SetSort(JsonSortRequest sort)
        {
            var sortKey = $"{sort.ModuleName}_sort_keys";
            var retSort = CookieHelper.GetValue<List<JsonSortValue>>(sortKey, false) ?? new List<JsonSortValue>();
            retSort.Add(new JsonSortValue
            {
                SortProperty = sort.SortProperty,
                SortDirect = sort.SortDirect
            });
            CookieHelper.CreateCookieJsonValue(retSort, sortKey);
            return Json(new VM_JsonOnlyResult
            {
                Result = true,
                Value = retSort
            });
        }

        public JsonResult RemoveSort(string moduleKey)
        {
            CookieHelper.RemoveIt($"{moduleKey}_sort_keys");
            return Json(new VM_JsonOnlyResult {Result = true});
        }

        [HttpPost]
        public Task<JsonResult> UploadFile(string uploadType = "resources")
        {
            if (Request.Files == null || Request.Files.Count == 0)
            {
                return Task.FromResult(Json(new VM_JsonOnlyResult { Result = false, Err = "No file requested" }));
            }
            return Task<JsonResult>.Factory.StartNew(() =>
            {
                var ret = new VM_JsonOnlyResult();
                var toUploadFile = Request.Files[0];
                if (toUploadFile == null)
                {
                    ret.Err = "File can't be found.";
                    return Json(ret);
                }
                if (toUploadFile.ContentLength > FileSize * 1024 * 1024)
                {
                    ret.Err = "File length is more than max limit.";
                    return Json(ret);
                }

                var toUploadFileInfo = new FileInfo(toUploadFile.FileName ?? "");
                var isToTranscoding = string.Compare(
                                          toUploadFileInfo.Extension, ".webm",
                                          StringComparison.OrdinalIgnoreCase) == 0;
                if (!isToTranscoding)
                {
                    var genFileName = uploadType + "_" + Guid.NewGuid().ToString() + toUploadFileInfo.Extension;
                    toUploadFile.SaveAs(SaveFilePath + genFileName);
                    ret.Value = VisitResUrl + genFileName;
                    ret.Result = true;
                    var qiniuUrl = QiniuStorageHelper.UploadFile(SaveFilePath + genFileName, uploadType, (file) =>
                    {
                        if (System.IO.File.Exists(file))
                        {
                            System.IO.File.Delete(file);
                        }
                    });
                    if (!string.IsNullOrEmpty(qiniuUrl))
                    {
                        ret.Value = qiniuUrl;
                    }
                }
                else
                {
                    var buffer = new byte[toUploadFile.ContentLength];
                    toUploadFile.InputStream.Read(buffer, 0, buffer.Length);
                    ret.Result = true;
                    ret.Value = QiniuStorageHelper.PutAudioFile(buffer, uploadType, Guid.NewGuid().ToString());
                }
                return Json(ret);
            });
        }
    }
}