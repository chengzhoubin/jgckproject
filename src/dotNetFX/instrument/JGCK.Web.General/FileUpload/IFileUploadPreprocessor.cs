using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace Happy.Web.Mvc.FileUpload
{
    /// <summary>
    /// 文件上传预处理器，如：提供水印功能。
    /// </summary>
    public interface IFileUploadPreprocessor
    {
        /// <summary>
        /// 预处理。
        /// </summary>
        void Process(HttpPostedFileBase file, ref Stream stream);
    }
}
