using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JGCK.Framework.EF;
using JGCK.Respority.UserWork;
using JGCK.Util.Enums;

namespace JGCK.Modules.Membership
{
    public class UserManager : AbstractMembershipService
    {
        public async Task<CheckUserPwdResult> CheckAsync(string userName, string pwd,
            Func<string, string> pwdMd5HashHandler = null)
        {
            var anUser = await userDbContext.Person.FirstOrDefaultAsync(user => user.Name == userName);
            return GetCheckingResult(anUser, pwd, pwdMd5HashHandler);
        }

        public CheckUserPwdResult Check(string userName, string pwd, Func<string, string> pwdMd5HashHandler = null)
        {
            var anUser = userDbContext.Person.FirstOrDefault(user => user.Name == userName);
            return GetCheckingResult(anUser, pwd, pwdMd5HashHandler);
        }

        private CheckUserPwdResult GetCheckingResult(Person inUser, string pwd, Func<string, string> pwdMd5HashHandler)
        {
            if (inUser == null)
                return CheckUserPwdResult.User_Not_Exist;
            var md5Pwd = pwdMd5HashHandler?.Invoke(pwd) ?? pwd;
            if (inUser.Pwd != md5Pwd)
                return CheckUserPwdResult.User_Pwd_Not_Match;
            return CheckUserPwdResult.Success;
        }

        public Task<List<Person>> GetStaffListAsync(
            Expression<Func<Person, bool>> search,
            AbstractUnitOfWork.OrderByExpression<Person>[] orderBy,
            int pageIndex)
        {
            var pager = new AbstractUnitOfWork.Pager {CurrentIndex = pageIndex};
            return userDbContext.GetObjectsAsync(search, pager, true, orderBy, p => p.Role);
        }

        public Task<int> GetStaffCount(Expression<Func<Person, bool>> search)
        {
            return userDbContext.Person.CountAsync(search);
        }

        public bool UserIsExists(string name)
        {
            return userDbContext.Person.Any(p => p.Name == name && !p.IsDeleted);
        }
    }
}
