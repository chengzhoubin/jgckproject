﻿using System;
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
        [JsonProperty("module")] public string ModuleName { get; set; }
    }

    [JsonObject()]
    public class JsonSortValue : ISortValue
    {
        [JsonProperty("sortby")] public string SortProperty { get; set; }

        [JsonProperty("directby")] public AscOrDesc SortDirect { get; set; }
    }
}