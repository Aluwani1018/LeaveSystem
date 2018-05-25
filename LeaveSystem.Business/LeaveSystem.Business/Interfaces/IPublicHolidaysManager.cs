using LeaveSystem.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Business.Interfaces
{
    public interface IPublicHolidaysManager
    {
        PublicHoliday GetByDate(DateTime date);
        IEnumerable<PublicHoliday> GetPublicHolidaysWithinRange(DateTime fromDate, DateTime toDate);

        int CalculateNumberOfDaysExcludingPublicHolidays(DateTime fromDate, DateTime toDate);
    }
}
