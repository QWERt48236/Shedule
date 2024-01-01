using System;
using System.IO;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using LiteDB;
using System.Collections.Generic;
using System.Windows.Media;

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

            Update_calendar(Current_date.Month);

            Year.Text = Current_date.Year.ToString();
            Month.Text = Dates.CurrentMonthName(Current_date.Month);
        }



        private void Last_month(object sender, EventArgs e)
        {
            DateTime New_Current_date = new DateTime(Current_date.Year, Current_date.Month - 1, 1);
            Current_date = New_Current_date;
            this.Year.Text = New_Current_date.Year.ToString();
            this.Month.Text = Dates.CurrentMonthName(New_Current_date.Month);

            Update_calendar(Current_date.Month);
        }



        private void Next_month(object sender, EventArgs e)
        {
            DateTime Next_month_date = new DateTime(Current_date.Year, Current_date.Month + 1, 1);
            Current_date = Next_month_date;
            this.Year.Text = Next_month_date.Year.ToString();
            this.Month.Text = Dates.CurrentMonthName(Next_month_date.Month);

            Update_calendar(Current_date.Month);
        }

        private void Update_calendar(int month)
        {
            Calendar_Grid.Children.Clear();
            int cnt = 0;
            Button[,] Button_arr = new Button[7, 7];
            string[] DateArr = Dates.GetDate(month);
            for (int i = 1; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    cnt++;
                    Button_arr[i, j] = new Button();
                    Button_arr[i, j].Content = DateArr[cnt];
                    Button_arr[i, j].Click += new RoutedEventHandler(Go_To_Task_list);
                    Calendar_Grid.Children.Add(Button_arr[i, j]);
                }
            }
        }

        private void Go_To_Task_list(object sender, EventArgs e)
        {

            Calendar_Grid.Visibility = Visibility.Collapsed;
            Month_back.Visibility = Visibility.Collapsed;
            Month_forvard.Visibility = Visibility.Collapsed;
            Stack.Visibility = Visibility.Visible;
            ReturnButton.Visibility = Visibility.Visible;

            Button Chosen_button = sender as Button;
            day = Convert.ToInt32(Chosen_button.Content);
            DateTime Chosen_date = new DateTime(Current_date.Year, Current_date.Month, day);

            DayData Chosen_day = DataBaseManaging.Get_Current_Day_Events(Chosen_date);

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

    }
}
