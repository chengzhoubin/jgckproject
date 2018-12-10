using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressMapper;
using JGCK.Respority.BasicInfo;
using JGCK.Respority.BasicInfo.Models;

namespace JGCK.Web.Admin.Models.Mapper
{
    public static class VmHospitalMapper
    {
        static VmHospitalMapper()
        {
            if (!ExpressMapper.Mapper.MapExists(typeof(Hospital), typeof(Hospital)))
            {
                ExpressMapper.Mapper.Register<Hospital, Hospital>().Before((h, t) =>
                {
                    t.HospitalInvoices?.Clear();
                    t.HospitalReferences?.Clear();
                });
            }

            if (!ExpressMapper.Mapper.MapExists(typeof(HospitalInvoice), typeof(HospitalInvoice)))
                ExpressMapper.Mapper.Register<HospitalInvoice, HospitalInvoice>();
            if (!ExpressMapper.Mapper.MapExists(typeof(HospitalReference), typeof(HospitalReference)))
                ExpressMapper.Mapper.Register<HospitalReference, HospitalReference>();
        }

        public static Hospital MapTo(this Hospital existHospital, Hospital targetHospital)
        {
            return ExpressMapper.Mapper.Map(existHospital, targetHospital);
        }
    }
}