using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Util.CloudStorage
{
    public class QiniuConfigurationElement
    {
        public string remark { get; set; }

        public string accessKey { get; set; }

        public string secretKey { get; set; }

        public List<bucketConfigurationElement> buckets { get; set; }
    }

    public class bucketConfigurationElement
    {
        public string remark { get; set; }

        public string id { get; set; }

        public string bucket { get; set; }

        public string baseURL { get; set; }
    }
}
