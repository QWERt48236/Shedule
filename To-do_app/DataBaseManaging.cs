using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace To_do_app
{
    public static class DataBaseManaging
    {
        public static DayData Get_Current_Day_Events(DateTime Chosen_date)
        {
            DayData Chosen_day;

            using (var db = new LiteDatabase(@"SheduleDB.db"))
            {
                //db.DropCollection("dayData");
                var col = db.GetCollection<DayData>("dayData");

                col.EnsureIndex(x => x.Date);
                Chosen_day = col.FindOne(x => x.Date == Chosen_date);
            }
            return Chosen_day;
        }
    }
}
