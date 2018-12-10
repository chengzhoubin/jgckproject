using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.CommandTrees;
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
            var sortKey = sort.ModuleName;//$"{sort.ModuleName}_sort_keys";
            var retSort = CookieHelper.GetValue<List<JsonSortValue>>(sortKey, false) ?? new List<JsonSortValue>();
            var existSort = retSort.FirstOrDefault(j => j.SortProperty == sort.SortProperty);
            if (existSort == null)
            {
                retSort.Add(new JsonSortValue
                {
                    SortProperty = sort.SortProperty,
                    SortDirect = sort.SortDirect
                });
            }
            else
            {
                existSort.SortDirect = sort.SortDirect;
            }

            CookieHelper.CreateCookieJsonValue(retSort, sortKey, isSecurity: false);
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

        /// <summary>
        /// 上传接口
        /// </summary>
        /// <param name="uploadType">format:region:module1:module2</param>
        /// <returns></returns>
        [HttpPost]
        public Task<JsonResult> UploadFile(string uploadType = "resources")
        {
            if (Request.Files == null || Request.Files.Count == 0)
            {
                return Task.FromResult(Json(new VM_JsonOnlyResult {Result = false, Err = "No file requested"}));
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

                var saveFileDir = GetOrCreateStorageDir(
                    LocalStorageConfiguration.Instance.UploadRootPath, uploadType);
                string outputFullFileName = "";

                var allowed = IsAllowedExtension(toUploadFile, saveFileDir, out outputFullFileName);
                if (!allowed)
                {
                    ret.Err = "File type is not allowed to upload.";
                    return Json(ret);
                }

                var outFile = new FileInfo(outputFullFileName);
                var visitUrl = LocalStorageConfiguration.Instance.VisitBaseUrl
                               + "/" + uploadType.Replace(":", "/")
                               + "/" + outFile.Name;
                ret.Result = true;
                ret.Value = visitUrl;
                return Json(ret);
            });
        }

        private static readonly object LockWithCreateDir = new object();

        private static readonly HashSet<string> AllowUploadFileExts =
            new HashSet<string>
            {
                "7173",
                "255216",
                "13780",
                "6677",
                "8075",
                "8297"
            };
        
        private string GetOrCreateStorageDir(string rootPath, string type)
        {
            var fileStorageDir = Path.Combine(rootPath, type.Replace(":", @"\"));
            if (Directory.Exists(fileStorageDir))
                return fileStorageDir;

            lock (LockWithCreateDir)
            {
                if (Directory.Exists(fileStorageDir))
                    return fileStorageDir;

                Directory.CreateDirectory(fileStorageDir);
                return fileStorageDir;
            }
        }

        private bool IsAllowedExtension(HttpPostedFileBase f, string fileDir, out string outputFileName)
        {
            FileInfo fi = new FileInfo(f.FileName);
            if (!LocalStorageConfiguration.Instance.AllowUploadFileExt.Any(ext => ext == fi.Extension))
            {
                goto skipRegion;
            }

            var newFile = Path.Combine(fileDir, Guid.NewGuid().ToString("D") + fi.Extension);
            f.SaveAs(newFile);

            var fs = new FileStream(newFile, FileMode.Open, FileAccess.Read);
            var r = new BinaryReader(fs);
            var buffer = r.ReadByte();
            var fileclass = buffer.ToString();
            buffer = r.ReadByte();
            fileclass += buffer.ToString();
            r.Close();
            fs.Close();

            if (AllowUploadFileExts.Contains(fileclass))
            {
                outputFileName = newFile;
                return true;
            }

            System.IO.File.Delete(newFile);
            skipRegion:
            outputFileName = "";
            return false;
        }
    }
}