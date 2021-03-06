﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework;
using JGCK.Respority.BasicInfo;
using JGCK.Respority.ProductWork;

namespace JGCK.Modules.Configuration
{
    public abstract class AbstractConfigurationService : AbstractDefaultAppService, ITransistService
    {
        protected BasicDbProxy basicDbContext { get; set; }

        protected ProductDbProxy productDbContext { get; set; }
    }
}
