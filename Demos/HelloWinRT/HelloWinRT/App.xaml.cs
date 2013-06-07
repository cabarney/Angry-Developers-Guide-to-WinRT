using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace HelloWinRT
{
    sealed partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Window.Current.Content = new MainView{DataContext = new MainViewModel()};
            Window.Current.Activate();
        }
    }
}
