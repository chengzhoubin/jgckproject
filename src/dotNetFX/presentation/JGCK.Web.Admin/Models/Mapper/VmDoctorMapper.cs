using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JGCK.Respority.UserWork;

namespace JGCK.Web.Admin.Models.Mapper
{
    public static class VmDoctorMapper
    {
        static VmDoctorMapper()
        {
            if (!ExpressMapper.Mapper.MapExists(typeof(Person), typeof(Person)))
                ExpressMapper.Mapper.Register<Person, Person>();
            if (!ExpressMapper.Mapper.MapExists(typeof(PersonDoctor), typeof(PersonDoctor)))
                ExpressMapper.Mapper.Register<PersonDoctor, PersonDoctor>().Before((s, t) => { t.InHospital?.Clear(); });
            if (!ExpressMapper.Mapper.MapExists(typeof(DoctorInHospital), typeof(DoctorInHospital)))
                ExpressMapper.Mapper.Register<DoctorInHospital, DoctorInHospital>();
        }

        public static Person MapTo(this Person sourcePerson, Person targetPerson)
        {
            return ExpressMapper.Mapper.Map(sourcePerson, targetPerson);
        }
    }
}