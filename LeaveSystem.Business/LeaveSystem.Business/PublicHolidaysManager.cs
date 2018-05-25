using LeaveSystem.Business.Interfaces;
using LeaveSystem.Data.Model;
using LeaveSystem.Data.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeaveSystem.Business
{
    public class PublicHolidaysManager : IPublicHolidaysManager
    {
        private readonly IUnitOfWork uow;
        public PublicHolidaysManager(IUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
        }

        public int CalculateNumberOfDaysExcludingPublicHolidays(DateTime fromDate, DateTime toDate)
        {

            throw new NotImplementedException();
        }

        public PublicHoliday GetByDate(DateTime date)
        {
            //make sure date is in the form of date not date time
            date = date.Date;
            return uow.PublicHolidays.GetWhere(x => x.Date.Day == date.Day && x.Date.Month == date.Month).FirstOrDefault();
        }

        public IEnumerable<PublicHoliday> GetPublicHolidaysWithinRange(DateTime fromDate, DateTime toDate)
        {
            //only need to campare Day and Month
            var numberOfDays = (toDate - fromDate).Days;
            List<PublicHoliday> publicHolidaysList = new List<PublicHoliday>();
            for (int day = 0; day < numberOfDays; day++)
            {
                var currentDay = fromDate.AddDays(day);
                var publicHoliday = uow.PublicHolidays
                    .GetWhere(x => x.Date.Day == currentDay.Day && x.Date.Month == currentDay.Month).FirstOrDefault();
                //is it a holiday
                if (publicHoliday != null)
                {
                    publicHolidaysList.Add(publicHoliday);
                }
                else
                {
                    //if not a holiday check if its a weekend
                    var dayOfTheWeek = currentDay.DayOfWeek;
                    if (dayOfTheWeek == DayOfWeek.Saturday || dayOfTheWeek == DayOfWeek.Sunday)
                        publicHolidaysList.Add(new PublicHoliday() { Name = "Weekend" });
                }
            }
            return publicHolidaysList;
        }
    }
}
