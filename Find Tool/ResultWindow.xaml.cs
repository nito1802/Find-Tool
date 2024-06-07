using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvertUrls
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(string content)
        {
            InitializeComponent();
            tbContent.Text = content;
        }

        private void TbContent_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 3)
            {
                TextBox tb = sender as TextBox;
                tb.SelectAll();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}