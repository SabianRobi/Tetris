using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace TetrisUWP
{
    public sealed partial class TutorialPage : Page
    {
        public TutorialPage()
        {
            InitializeComponent();

            //GoBack button windows
            var view = SystemNavigationManager.GetForCurrentView();
            view.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            view.BackRequested += GoBack;
        }

        private void GoBack(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
    }
}
