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
        public string HiddenJsonString => JsonConvert.SerializeObject(this);
    }
}
