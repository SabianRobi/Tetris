using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TetrisUWP
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }

        private void btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void btn_Tutorial_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TutorialPage));
        }
    }
}
