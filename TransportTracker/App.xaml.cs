using Microsoft.Maui.Controls;
using TransportTracker.Services;

namespace TransportTracker
{
    public partial class App : Application
    {
        private readonly DatabaseService _databaseService;
        public App(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            MainPage = new AppShell();
        }        
        protected override async void OnStart()
        {
            base.OnStart();
            try
            {
                await _databaseService.InitializeAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось инициализировать базу данных: {ex.Message}", "OK");
            }
        }
    }
}