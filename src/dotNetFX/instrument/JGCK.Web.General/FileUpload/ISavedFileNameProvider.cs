using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;

namespace Happy.Web.Mvc.FileUpload
{
    /// <summary>
    /// 要保存的文件名提供者。
    /// </summary>
    public interface ISavedFileNameProvider
    {
        /// <summary>
        /// 返回保存到服务器的文件名。
        /// </summary>
        string GetSavedFileName(HttpPostedFileBase file);
    }
}
