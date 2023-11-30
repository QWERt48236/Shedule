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
            DateTime currentDate = new DateTime(DateTime.Now.Year, month, DateTime.Now.Day);
            string[] dateArr = new string[43];

            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

            int cnt = Convert.ToInt32(firstDayOfMonth.DayOfWeek);
            for (int i = cnt; i < cnt + DateTime.DaysInMonth(currentDate.Year, currentDate.Month)+6; i++)
            {

                if((i - Convert.ToInt32(firstDayOfMonth.DayOfWeek) - 6) > 0)
                {
                    dateArr[i] = Convert.ToString(i - Convert.ToInt32(firstDayOfMonth.DayOfWeek) - 6);
                }
                
            }

            bool check = false;
            for (int i = 0; i <= 7; i++)
            {
                if (dateArr[i] == null) {check = true; }
                else { check = false; }
                
            }
            if(check) { dateArr = dateArr.Skip(7).ToArray(); }

            return dateArr;
        }


    }



    public class DayData
    {
        public DateTime Date { get; set; }
        public List<string> EventArr { get; set; }
    }

}
