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
        public string UploadRootPath => this.GetValue("uploadRootPath");
    }
}
