using System;
using System.IO;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using LiteDB;
using System.Collections.Generic;

namespace To_do_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public static class Dates
    {
        public static string CurrentMonthName()
        {
            int month = DateTime.Now.Month;
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return months[month - 1];
        }

        //returns array of dates (int values) which should appear in calendar grid
        public static int[] GetDate()
        {
            DateTime currentDate = DateTime.Now;
            int[] dateArr = new int[43];
            DateTime pastMonth;

            if (currentDate.Month == 1)
            {
                pastMonth = new DateTime(currentDate.Year - 1, 12, DateTime.DaysInMonth(currentDate.Year - 1, 12));
            }
            else
            {
                pastMonth = new DateTime(currentDate.Year, currentDate.Month - 1, DateTime.DaysInMonth(currentDate.Year, currentDate.Month - 1));
            }

            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            int cnt = pastMonth.Day;
            for (int i = 0; i <= Convert.ToInt32(firstDayOfMonth.DayOfWeek); i++)
            {
                dateArr[i] = cnt + i - Convert.ToInt32(firstDayOfMonth.DayOfWeek);
            }

            cnt = Convert.ToInt32(firstDayOfMonth.DayOfWeek) + 1;
            for (int i = cnt; i <= cnt + DateTime.DaysInMonth(currentDate.Year, currentDate.Month); i++)
            {
                dateArr[i] = i - Convert.ToInt32(firstDayOfMonth.DayOfWeek);
            }

            cnt = DateTime.DaysInMonth(currentDate.Year, currentDate.Month) + Convert.ToInt32(firstDayOfMonth.DayOfWeek);
            for (int i = 1; i < 43 - cnt; i++)
            {
                dateArr[cnt + i] = i;
            }

            return dateArr;
        }
    }


    class DayData
    {
        public DateTime Date { get; set; }
        public List<string> EventArr { get; set; }
    }



    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        private int day;

        public MainWindow()
        {
            InitializeComponent();

            Button[,] Button_arr = new Button[7, 7];
            int cnt = 0;
            int[] DateArr = Dates.GetDate();
            for (int i = 1; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    cnt++;
                    Button_arr[i, j] = new Button();
                    Button_arr[i, j].Content = DateArr[cnt];
                    Button_arr[i, j].Click += new RoutedEventHandler(Go_To_Task_list);
                    Main_Calendar.Children.Add(Button_arr[i, j]);
                }
            }
            Year.Text = DateTime.Now.Year.ToString();
            Month.Text = Dates.CurrentMonthName();
        }


        private void Go_To_Task_list(object sender, EventArgs e)
        {

            Main_Calendar.Visibility = Visibility.Collapsed;
            Stack.Visibility = Visibility.Visible;
            ReturnButton.Visibility = Visibility.Visible;

            Button Chosen_button = sender as Button;
            day = Convert.ToInt32(Chosen_button.Content);
            DateTime Chosen_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
            DayData Chosen_day;

            using (var db = new LiteDatabase(@"SheduleDB.db"))
            {
                //db.DropCollection("dayData");
                var col = db.GetCollection<DayData>("dayData");

                col.EnsureIndex(x => x.Date);
                Chosen_day = col.FindOne(x => x.Date == Chosen_date);
            }

            if (Chosen_day != null)
            {
                List<String> EventBlock = Chosen_day.EventArr;

                for (int i = 1; i <= Chosen_day.EventArr.Count - 1; i++)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = EventBlock[i];
                    Stack.Children.Add(textBlock);
                }
            }
        }


        private void Event_block_Click(object sender, RoutedEventArgs e)
        {
            Event_window event_Window = new();
            event_Window.Text_block = sender as TextBlock;
            event_Window.Stack = Stack;
            event_Window.Show();
        }


        private void Enter_event(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string Event = event_name.Text;
                TextBlock Event_block = new();
                Event_block.Text = Event;
                Event_block.MouseDown += new MouseButtonEventHandler(Event_block_Click);
                Stack.Children.Add(Event_block);
                event_name.Clear();
            }
        }


        private void Return_to_calendar(object sender, RoutedEventArgs e)
        {
            DateTime Chosen_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);

            using (var db = new LiteDatabase(@"SheduleDB.db"))
            {
                var col = db.GetCollection<DayData>("dayData");
                DayData Chosen_day = new DayData();
                Chosen_day.EventArr = new List<string>();
                //db.DropCollection("dayData");

                if (col.FindOne(x => x.Date == Chosen_date) != null) //not remembering the tasks
                {
                    Chosen_day = col.FindOne(x => x.Date == Chosen_date);

                    for (int i = 1; i < Stack.Children.Count; i++)
                    {
                        TextBlock textBlock = Stack.Children[i] as TextBlock;
                        Chosen_day.EventArr.Add(textBlock.Text);
                    }

                    col.Update(Chosen_date, Chosen_day);
                }
                else
                {
                    Chosen_day.Date = Chosen_date;
                    Chosen_day.EventArr = new List<string>();
                    for (int i = 1; i < Stack.Children.Count; i++)
                    {
                        Chosen_day.EventArr.Add((Stack.Children[i] as TextBlock).Text);
                    }
                    var daydata = new DayData
                    {
                        Date = Chosen_date,
                        EventArr = Chosen_day.EventArr
                    };
                    col.Insert(daydata);

                }

                TextBox event1 = event_name;
                Stack.Children.Clear();
                Stack.Children.Add(event1);

                Main_Calendar.Visibility = Visibility.Visible;
                Stack.Visibility = Visibility.Collapsed;
                ReturnButton.Visibility = Visibility.Collapsed;
            }
        }

    }
}
