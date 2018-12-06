using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressMapper;
using JGCK.Respority.BasicInfo;

namespace JGCK.Web.Admin.Models.Mapper
{
    public static class VmHospitalMapper
    {
        static VmHospitalMapper()
        {
            ExpressMapper.Mapper.Register<Hospital, Hospital>().Before((h, t) =>
            {
                t.HospitalInvoices?.Clear();
                t.HospitalReferences?.Clear();
            });
        }

        public static Hospital MapTo(this Hospital existHospital)
        {
            //ExpressMapper.Mapper.Map<Hospital,Hospital>(existHospital)
            return null;
        }
    }
}