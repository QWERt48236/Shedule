using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace To_do_app
{

    public static class Dates
    {
        public static string CurrentMonthName(int month)
        {
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return months[month - 1];
        }


        /// <summary>
        /// gets array of dates that will appear in calendar grid
        /// </summary>
        public static string[] GetDate(int month)
        {
            
            DateTime currentDate = DateTime.Now;
            string[] dateArr = new string[36];


            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);


            int cnt = Convert.ToInt32(firstDayOfMonth.DayOfWeek);
            for (int i = cnt; i < cnt + DateTime.DaysInMonth(currentDate.Year, currentDate.Month); i++)
            {
                dateArr[i] = Convert.ToString(i - Convert.ToInt32(firstDayOfMonth.DayOfWeek) + 1);
            }

            cnt = DateTime.DaysInMonth(currentDate.Year, currentDate.Month) + Convert.ToInt32(firstDayOfMonth.DayOfWeek);


            return dateArr;
        }
    }



    class DayData
    {
        public DateTime Date { get; set; }
        public List<string> EventArr { get; set; }
    }

}
