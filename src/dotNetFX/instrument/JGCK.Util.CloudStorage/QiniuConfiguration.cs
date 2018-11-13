using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework;

namespace JGCK.Util.CloudStorage
{
    internal class QiniuConfiguration : AbstractDefaultConfiguration<QiniuConfiguration>
    {
        public string ConfigFile => this.GetValue("qiniu_StorageBucketConfigFile");

        public string PutPolicyTimeout => this.GetValue("qiniu_PutPolicyTimeout");

        public string DefaultBucketName => this.GetValue("qiniu_DefaultBucketName");

        public bool IsRunning => Convert.ToBoolean(GetValue("qiniu_IsRunning"));
    }
}
