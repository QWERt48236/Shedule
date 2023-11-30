using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace To_do_app
{
    public partial class MainWindow : Window
    {

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
            DateTime Chosen_date = new DateTime(Current_date.Year, Current_date.Month, day);

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
                        Chosen_day.EventArr.Add((Stack.Children[i] as TextBlock).Text);
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

                Calendar_Grid.Visibility = Visibility.Visible;
                Month_back.Visibility = Visibility.Visible;
                Month_forvard.Visibility = Visibility.Visible;
                Stack.Visibility = Visibility.Collapsed;
                ReturnButton.Visibility = Visibility.Collapsed;
            }
        }

    }
}
