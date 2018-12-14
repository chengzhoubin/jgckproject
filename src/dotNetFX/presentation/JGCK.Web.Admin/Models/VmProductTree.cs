using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JGCK.Web.Admin.Models
{
    public class VmProductTree
    {
        public string Name { get; set; }

        public long Id { get; set; }

        public IList<ProductTreeChildren> children { get; set; }
}

    public class ProductTreeChildren
    {
        public string Name { get; set; }

        public long Id { get; set; }
    }
}