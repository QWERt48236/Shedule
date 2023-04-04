using System.Windows;
using System.Windows.Controls;

namespace Calendar
{
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
