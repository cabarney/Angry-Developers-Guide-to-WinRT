namespace HelloSilverlight
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            Startup += (o, e) => RootVisual = new MainView {DataContext = new MainViewModel()};
        }
    }
}
