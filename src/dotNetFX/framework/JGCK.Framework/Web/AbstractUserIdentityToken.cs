using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Framework.Web
{
    public abstract class AbstractUserIdentityToken<T> : IToken<T>
    {
        public string UserID { get; set; }

        public string UserName { get; set; }

        public string RoleID { get; set; }

        public string RoleName { get; set; }

        public virtual string TokenName { get; } 

        protected abstract Action<IToken<T>> BuildHandler { get; }

        protected abstract Func<T> ResolveHandler { get; }


        public abstract void BuildToken();

        public abstract T ResolveToken();
    }
}
