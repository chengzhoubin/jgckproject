using JGCK.Respority.UserWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JGCK.Web.Admin.Models.Mapper
{
    public static class VmPersonMapper
    {
        static VmPersonMapper()
        {
            if (!ExpressMapper.Mapper.MapExists(typeof(Person), typeof(Person)))
                ExpressMapper.Mapper.Register<Person, Person>().Before((s, t) => t.Role?.Person?.Clear());
        }

        public static Person MapTo(this Person existPerson, Person targetPerson)
        {
            return ExpressMapper.Mapper.Map(existPerson, targetPerson);
        }
    }
}