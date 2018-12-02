using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JGCK.Web.General.MVC
{
    public abstract class AbstractVO
    {
        [JsonIgnore]
        public Action ResetSettingHandler { set; private get; }

        [JsonIgnore]
        public string HiddenJsonString
        {
            get
            {
                ResetSettingHandler?.Invoke();
                return JsonConvert.SerializeObject(this); 
            }
        }
    }
}
