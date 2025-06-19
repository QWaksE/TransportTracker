using TransportTracker.Models;
using TransportTracker.Services;

namespace TransportTracker
{
    public partial class WorkPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public WorkPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadWorkAsync();
        }
        private async void LoadWorkAsync()
        {
            try
            {
                var connection = _databaseService.GetConnection();
                var workList = await connection.Table<Work>().ToListAsync();
                var routes = await connection.Table<Route>().ToListAsync();
                var drivers = await connection.Table<Driver>().ToListAsync();
                if (workList.Count == 0 && routes.Any() && drivers.Any())
                {
                    var testWork = new List<Work>
                    {
                        new Work
                        {
                            RouteId = routes[0].Id,
                            DriverId = drivers[0].Id,
                            SecondDriverId = drivers.Count > 1 ? drivers[1].Id : null,
                            DepartureDate = DateTime.Today.AddDays(-5),
                            ReturnDate = DateTime.Today.AddDays(-3),
                            Bonus = 2000m
                        },
                        new Work
                        {
                            RouteId = routes[routes.Count > 1 ? 1 : 0].Id,
                            DriverId = drivers[drivers.Count > 1 ? 1 : 0].Id,
                            DepartureDate = DateTime.Today.AddDays(-2),
                            ReturnDate = DateTime.Today,
                            Bonus = 1000m
                        }
                    };
                    await connection.InsertAllAsync(testWork);
                    workList = await connection.Table<Work>().ToListAsync();
                }
                foreach (var work in workList)
                {
                    var route = routes.FirstOrDefault(r => r.Id == work.RouteId);
                    var driver1 = drivers.FirstOrDefault(d => d.Id == work.DriverId);
                    var driver2 = work.SecondDriverId.HasValue ? drivers.FirstOrDefault(d => d.Id == work.SecondDriverId.Value) : null;
                    work.RouteName = route?.Name ?? "Неизвестный маршрут";
                    work.DriverFullName = driver1?.FullName ?? "Неизвестный водитель";
                    work.SecondDriverFullName = driver2?.FullName;
                    work.RoutePayment = route?.Payment ?? 0;
                    work.DriverExperience = driver1?.Experience ?? 0;
                    work.SecondDriverExperience = driver2?.Experience;
                }
                WorkList.ItemsSource = workList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось загрузить работы: {ex.Message}", "OK");
            }
        }
        private async void OnAddWorkButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddWorkPage(_databaseService));
        }
        private async void OnWorkSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Work selectedWork)
            {
                await Navigation.PushAsync(new AddWorkPage(_databaseService, selectedWork));
                WorkList.SelectedItem = null;
            }
        }
        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Work work)
            {
                bool confirm = await DisplayAlert("Подтверждение", $"Удалить работу по маршруту '{work.RouteName}'?", "Да", "Нет");
                if (confirm)
                {
                    try
                    {
                        await _databaseService.GetConnection().DeleteAsync(work);
                        await DisplayAlert("Успех", "Работа удалена.", "OK");
                        LoadWorkAsync();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Ошибка", $"Не удалось удалить работу: {ex.Message}", "OK");
                    }
                }
            }
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