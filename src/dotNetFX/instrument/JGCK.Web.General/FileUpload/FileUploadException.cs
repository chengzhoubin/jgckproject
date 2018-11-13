using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Happy.Web.Mvc.FileUpload
{
    /// <summary>
    /// 文件上传异常。
    /// </summary>
    public sealed class FileUploadException : ApplicationException
    {
        /// <summary>
        /// 构造方法。
        /// </summary>
        public FileUploadException(string message)
            : base(message)
        { 
        }
    }
}
