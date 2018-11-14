using JGCK.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Respority.BasicInfo;

namespace JGCK.Modules.Configuration
{
    public class WorktimeManager : AbstractConfigurationService
    {
        public async Task<IEnumerable<OffDay>> GetOffDays(int year, int startMonth, int? endMonth = null)
        {
            if (year <= 0 || startMonth <= 0 || (endMonth.HasValue && endMonth <= 0))
                return null;
            var chooseStartDate = Convert.ToDateTime(year + "-" + startMonth + "-01");
            var query = basicDbContext.OffDay.Where(day => day.NonworkDate >= chooseStartDate);
            if (endMonth.HasValue)
            {
                var chooseEndDate = Convert.ToDateTime(year + "-" + endMonth + "-31");
                query = query.Where(day => day.NonworkDate <= chooseEndDate);
            }

            var restDays = await query.ToListAsync();
            if (restDays?.Count > 0)
                return restDays;
            return GetDefaultRestDays(year);
        }

        private IEnumerable<OffDay> GetDefaultRestDays(int year)
        {
            DateTime counYear = Convert.ToDateTime($"{year}-01-01");
            DateTime nestYear = counYear.AddYears(1);
            for (DateTime i = counYear; i < nestYear; i = i.AddDays(1))
            {
                if (i.DayOfWeek == DayOfWeek.Sunday || i.DayOfWeek == DayOfWeek.Saturday)
                {
                    yield return new OffDay()
                    {
                        NonworkDate = i
                    };
                }
            }
        }
    }
}
