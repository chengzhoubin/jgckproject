using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JGCK.Util.Enums;
using JGCK.Web.General.VO;
using Newtonsoft.Json;

namespace JGCK.Web.Admin.Models
{
    public class JsonSortRequest : JsonSortValue
    {
        public string ModuleName { get; set; }
    }

    [JsonObject()]
    public class JsonSortValue : ISortValue
    {
        public string SortProperty { get; set; }

        public AscOrDesc SortDirect { get; set; }
    }
}