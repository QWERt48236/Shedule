using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace To_do_app
{
    /// <summary>
    /// Interaction logic for Event_window.xaml
    /// </summary>
    public partial class Event_window : Window
    {


        private TextBlock text_block;
        public TextBlock Text_block
        {
            get { return text_block; }
            set { text_block = value; }
        }

        private StackPanel stack;
        public StackPanel Stack
        {
            get { return stack; }
            set { stack = value; }
        }



        public Event_window()
        {
            InitializeComponent();
        }

        private void Delete_block(object sender, RoutedEventArgs e)
        {
            stack.Children.Remove(text_block);
            this.Close();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
