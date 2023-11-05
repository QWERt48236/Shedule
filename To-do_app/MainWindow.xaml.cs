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


    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        private int day;
        private DateTime Current_date;
        public MainWindow()
        {
            InitializeComponent();
            Current_date = DateTime.Now;
            Button[,] Button_arr = new Button[7, 7];
            int cnt = 0;
            string[] DateArr = Dates.GetDate(Current_date.Month);
            for (int i = 1; i < 6; i++)
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
            Year.Text = Current_date.Year.ToString();
            Month.Text = Dates.CurrentMonthName(Current_date.Month);
        }



        private void Last_month(object sender, EventArgs e)
        {
            DateTime New_Current_date = new DateTime(Current_date.Year, Current_date.Month - 1, 1);
            Current_date = New_Current_date;
            this.Year.Text = New_Current_date.Year.ToString();
            this.Month.Text = Dates.CurrentMonthName(New_Current_date.Month);
        }



        private void Next_month(object sender, EventArgs e)
        {
            DateTime Next_month_date = new DateTime(Current_date.Year, Current_date.Month + 1, 1);
            Current_date = Next_month_date;
            this.Year.Text = Next_month_date.Year.ToString();
            this.Month.Text = Dates.CurrentMonthName(Next_month_date.Month);
        }

        private void Go_To_Task_list(object sender, EventArgs e)
        {

            Main_Calendar.Visibility = Visibility.Collapsed;
            Month_back.Visibility = Visibility.Collapsed;
            Month_forvard.Visibility = Visibility.Collapsed;
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

                for (int i = 0; i <= Chosen_day.EventArr.Count - 1; i++)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = EventBlock[i];
                    textBlock.MouseDown += new MouseButtonEventHandler(Event_block_Click);
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
                Chosen_day.Date = Chosen_date;
                Chosen_day.EventArr = new List<string>();

                if (col.FindOne(x => x.Date == Chosen_date) != null)
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
                Month_back.Visibility = Visibility.Visible;
                Month_forvard.Visibility = Visibility.Visible;
                Stack.Visibility = Visibility.Collapsed;
                ReturnButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
