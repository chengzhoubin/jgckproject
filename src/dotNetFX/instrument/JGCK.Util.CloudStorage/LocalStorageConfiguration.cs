using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework;

namespace JGCK.Util.CloudStorage
{
    public class LocalStorageConfiguration : AbstractDefaultConfiguration<LocalStorageConfiguration>
    {
        public string UploadRootPath => this.GetValue("saveUploadedFilePath");

        public string VisitBaseUrl => this.GetValue("visitUploadedFileUrl");

        public IEnumerable<string> AllowUploadFileExt
        {
            get
            {
                var ret = new List<string>();
                var fileExts = this.GetValue("allowFileExt");
                if (string.IsNullOrEmpty(fileExts))
                    return ret;
                ret.AddRange(fileExts.Split('|'));
                return ret;
            }
        } 
    }
}
