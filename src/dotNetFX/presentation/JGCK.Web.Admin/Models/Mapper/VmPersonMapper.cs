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
            ExpressMapper.Mapper.Register<Person, Person>().Before((h, t) =>
            {

            });
        }

        public static Person MapTo(this Person existPerson, Person targetPerson)
        {
            return ExpressMapper.Mapper.Map(existPerson, targetPerson);
        }
    }
}