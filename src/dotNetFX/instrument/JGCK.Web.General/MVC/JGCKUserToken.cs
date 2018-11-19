using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework.Web;

namespace JGCK.Web.General.MVC
{
    public class JGCKUserToken : AbstractUserIdentityToken<JGCKUserToken>
    {
        public override string TokenName => "jgck_user_token";

        protected override Action<IToken<JGCKUserToken>> BuildHandler => token =>
        {
            var jgckUserToken = (JGCKUserToken) token;
            CookieHelper.CreateCookieJsonValue(jgckUserToken, TokenName);
        };

        protected override Func<JGCKUserToken> ResolveHandler => () =>
        {
            try
            {
                return CookieHelper.GetValue<JGCKUserToken>(TokenName);
            }
            catch
            {
                return null;
            }
        };

        public override void BuildToken()
        {
            this.BuildHandler?.Invoke(this);
        }

        public override JGCKUserToken ResolveToken()
        {
            return this.ResolveHandler?.Invoke();
        }

        public static JGCKUserToken ResolveNewToken()
        {
            var token = new JGCKUserToken();
            return token.ResolveHandler();
        }
    }
}
