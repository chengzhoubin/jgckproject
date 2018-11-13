using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Happy.ExtentionMethods;

namespace Happy.Web.Mvc.FileUpload
{
    /// <summary>
    /// 文件上传结果。
    /// </summary>
    public sealed class FileUploadResult
    {
        /// <summary>
        /// 构造方法。
        /// </summary>
        public FileUploadResult(string uploadedFileName, string savedFileName)
        {
            //uploadedFileName.MustNotNullAndNotWhiteSpace("uploadedFileName");
            //savedFileName.MustNotNullAndNotWhiteSpace("savedFileName");

            this.UploadedFileName = uploadedFileName;
            this.SavedFileName = savedFileName;
        }

        /// <summary>
        /// 客户端上传的文件名。
        /// </summary>
        public string UploadedFileName { get; private set; }

        /// <summary>
        /// 服务器保存的文件名。
        /// </summary>
        public string SavedFileName { get; private set; }
    }
}
