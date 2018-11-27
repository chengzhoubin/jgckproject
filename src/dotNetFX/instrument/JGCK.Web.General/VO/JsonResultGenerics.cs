using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Web.General.VO
{
    public class JsonResultGenerics<TValue>
    {
        public bool Result { get; set; } = true;

        public string Error { get; set; }

        public TValue Value { get; set; }
    }
}
