using TransportTracker.Models;
using TransportTracker.Services;

namespace TransportTracker
{
    public partial class RoutesPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public RoutesPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadRoutesAsync();
        }
        private async void LoadRoutesAsync()
        {
            try
            {
                var connection = _databaseService.GetConnection();
                var routes = await connection.Table<Route>().ToListAsync();
                if (routes.Count == 0)
                {
                    var testRoutes = new List<Route>
                    {
                        new Route { Name = "Москва - Санкт-Петербург", Distance = 700.5, DaysInTransit = 2, Payment = 15000m },
                        new Route { Name = "Москва - Казань", Distance = 820.0, DaysInTransit = 3, Payment = 18000m },
                        new Route { Name = "Санкт-Петербург - Новосибирск", Distance = 3100.0, DaysInTransit = 5, Payment = 35000m }
                    };

                    await connection.InsertAllAsync(testRoutes);
                    routes = await connection.Table<Route>().ToListAsync();
                }
                RoutesList.ItemsSource = routes;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось загрузить маршруты: {ex.Message}", "OK");
            }
        }
        private async void OnAddRouteButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddRoutePage(_databaseService));
        }
        private async void OnRouteSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Route selectedRoute)
            {
                await Navigation.PushAsync(new AddRoutePage(_databaseService, selectedRoute));
                RoutesList.SelectedItem = null;
            }
        }
        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Route route)
            {
                bool confirm = await DisplayAlert("Подтверждение", $"Удалить маршрут '{route.Name}'?", "Да", "Нет");
                if (confirm)
                {
                    try
                    {
                        await _databaseService.GetConnection().DeleteAsync(route);
                        await DisplayAlert("Успех", "Маршрут удалён.", "OK");
                        LoadRoutesAsync();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Ошибка", $"Не удалось удалить маршрут: {ex.Message}", "OK");
                    }
                }
            }
        }
        private async void OnWorkButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//WorkPage");
        }

    }
}
