using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

//using Happy.ExtentionMethods;
//using Happy.Utils;

namespace Happy.Web.Mvc.FileUpload
{
    /// <summary>
    /// 基于 Guid 的文件名提供者。
    /// </summary>
    public sealed class GuidSavedFileNameProvider : ISavedFileNameProvider
    {
        private readonly string _baseRelativeUrl;

        /// <summary>
        /// 构造方法。
        /// </summary>
        public GuidSavedFileNameProvider(string baseRelativeUrl)
        {
            //baseRelativeUrl.MustNotNullAndNotWhiteSpace("baseRelativeUrl");

            _baseRelativeUrl = baseRelativeUrl;
        }

        /// <inheritdoc />
        public string GetSavedFileName(HttpPostedFileBase file)
        {
            var guidStr = Guid.NewGuid().ToString().Replace('-', '_');
            var fileName = Path.GetFileName(file.FileName);
            var baseDirectory = HttpContext.Current.Server.MapPath(_baseRelativeUrl);

            return Path.Combine(baseDirectory, guidStr + "_" + fileName);
        }
    }
}
