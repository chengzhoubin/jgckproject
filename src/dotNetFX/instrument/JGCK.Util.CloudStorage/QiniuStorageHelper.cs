using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;

namespace JGCK.Util.CloudStorage
{
    public static class QiniuStorageHelper
    {
        private static readonly QiniuConfigurationElement m_Config;

        private static Mac CurMac => new Mac(m_Config.accessKey, m_Config.secretKey);

        private static int PolicyTimeout => Convert.ToInt32(QiniuConfiguration.Instance.PutPolicyTimeout ?? "3600");

        private static Config CurConfig
        {
            get
            {
                return new Config
                {
                    Zone = Zone.ZONE_CN_East,
                    UseHttps = true,
                    UseCdnDomains = true,
                    ChunkSize = ChunkUnit.U512K
                };
            }
        }

        static QiniuStorageHelper()
        {
            if (!QiniuConfiguration.Instance.IsRunning)
            {
                return;
            }

            if (!File.Exists(QiniuConfiguration.Instance.ConfigFile))
            {
                throw new FileNotFoundException();
            }

            string qiniuJsonString = "";
            using (var fs = new StreamReader(QiniuConfiguration.Instance.ConfigFile))
            {
                qiniuJsonString = fs.ReadToEnd();
            }
            m_Config = JsonConvert.DeserializeObject<QiniuConfigurationElement>(qiniuJsonString);
            if (m_Config == null)
            {
                throw new NullReferenceException();
            }
        }

        public static string UploadFile(
            string serverFilePath,
            string bucketId = "resources",
            Action<string> afterUploadedHandle = null)
        {
            if (!QiniuConfiguration.Instance.IsRunning || m_Config == null)
                return "";

            Mac mac = CurMac;
            var localFile = serverFilePath;
            var file = new System.IO.FileInfo(localFile);

            var bucket = GetBucket(bucketId);
            PutPolicy putPolicy = new PutPolicy
            {
                Scope = bucket.bucket
            };
            var iTimeout = PolicyTimeout;
            if (iTimeout > 0)
            {
                putPolicy.SetExpires(iTimeout);
            }

            var jstr = putPolicy.ToJsonString();
            var token = Auth.CreateUploadToken(mac, jstr);
            FormUploader target = new FormUploader(CurConfig);
            target.UploadFile(localFile, file.Name, token, null);
            afterUploadedHandle?.Invoke(localFile);
            return bucket.baseURL + "/" + file.Name;
        }

        public static string PutAudioFile(byte[] datas, string bucketId, string key)
        {
            var bucket = GetBucket(bucketId);
            var finalDocument = bucketId + "_" + key;
            var str = Base64.UrlSafeBase64Encode(bucket.bucket + ":" + finalDocument + ".mp3");
            var policy = new PutPolicy
            {
                Scope = bucket.bucket + ":" + finalDocument,
                PersistentPipeline = "webmTomp3",
                PersistentOps = "avthumb/mp3|saveas/" + str,
                PersistentNotifyUrl = "http://fake.com/qiniu/notify"
            };
            policy.SetExpires(PolicyTimeout);
            var upToken = Auth.CreateUploadToken(CurMac, policy.ToJsonString());
            var dictionary = new Dictionary<string, string>
            {
                {"scope", policy.Scope},
                {"persistentPipeline", policy.PersistentPipeline},
                {"persistentOps", policy.PersistentOps},
                {"persistentNotifyUrl", policy.PersistentNotifyUrl}
            };
            var extra = new PutExtra
            {
                Params = dictionary
            };
            var target = new UploadManager(CurConfig);
            var putStream = new MemoryStream(datas);
            target.UploadStream(putStream, finalDocument, upToken, extra);

            return bucket.baseURL + "/" + finalDocument + ".mp3";
        }

        private static bucketConfigurationElement GetBucket(string Id)
        {
            var bucketCount = m_Config?.buckets?.Count;
            var oneBucket = m_Config?.buckets?.FirstOrDefault(b => b.id == Id);
            if (oneBucket == null)
            {
                throw new NullReferenceException();
            }
            return oneBucket;
        }
    }
}
