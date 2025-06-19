namespace TransportTracker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void OnWorkButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//WorkPage");
        }
        private async void OnRoutesButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//RoutesPage");
        }
        private async void OnDriversButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//DriversPage");
        }
    }
}