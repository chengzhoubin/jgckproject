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

        public async Task<int> RemoveCurrentYearRestDays(int year)
        {
            var startDate = Convert.ToDateTime(year + "-01-01");
            var endDate = Convert.ToDateTime((year + 1) + "-01-01");
            var offLineDays =
                await basicDbContext.OffDay.Where(offline =>
                        offline.NonworkDate >= startDate &&
                        offline.NonworkDate < endDate)
                    .ToListAsync();
            if (offLineDays.Count > 0)
            {
                basicDbContext.OffDay.RemoveRange(offLineDays);
                return await basicDbContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> AddNewYearRestDays(IEnumerable<OffDay> Days)
        {
            basicDbContext.OffDay.AddRange(Days);
            return await basicDbContext.SaveChangesAsync();
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
