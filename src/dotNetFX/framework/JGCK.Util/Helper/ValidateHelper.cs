using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Util.Helper
{
    public class ValidateHelper
    {
        private static readonly object LockFlag = new object();
        private static IList<String> _sensitiveWords;
        private static Lazy<Task<IList<String>>> _lazyOfSensitiveWords;

        public static async Task<string> ReplaceSensitiveWords(
            Func<Task<IList<String>>> getSensitiveWordsFunc,
            String orginalString,
            String replaceString = "***")
        {
            if (string.IsNullOrEmpty(orginalString))
            {
                return await Task.FromResult("");
            }

            if (_lazyOfSensitiveWords == null)
            {
                lock (LockFlag)
                {
                    if (_lazyOfSensitiveWords == null)
                    {
                        _lazyOfSensitiveWords = new Lazy<Task<IList<String>>>(getSensitiveWordsFunc, true);
                    }
                }
            }
            if (!_lazyOfSensitiveWords.IsValueCreated)
            {
                _sensitiveWords = await _lazyOfSensitiveWords.Value;
            }
            ((List<String>) _sensitiveWords).ForEach(s =>
            {
                orginalString = orginalString.Replace(s, replaceString);
            });
            return orginalString;
        }
    }
}
