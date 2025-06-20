using TransportTracker.Models;
using TransportTracker.Services;

namespace TransportTracker
{
    public partial class DriversPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public DriversPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadDriversAsync();
        }
        private async void LoadDriversAsync()
        {
            try
            {
                var connection = _databaseService.GetConnection();
                var drivers = await connection.Table<Driver>().ToListAsync();

                if (drivers.Count == 0)
                {
                    var testDrivers = new List<Driver>
                    {
                        new Driver { LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Experience = 5 },
                        new Driver { LastName = "Петров", FirstName = "Пётр", MiddleName = "Сергеевич", Experience = 8 },
                        new Driver { LastName = "Сидоров", FirstName = "Алексей", Experience = 3 }
                    };

                    await connection.InsertAllAsync(testDrivers);
                    drivers = await connection.Table<Driver>().ToListAsync();
                }

                DriversList.ItemsSource = drivers;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось загрузить водителей: {ex.Message}", "OK");
            }
        }
        private async void OnAddDriverButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddDriverPage(_databaseService));
        }
        private async void OnDriverSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Driver selectedDriver)
            {
                await Navigation.PushAsync(new AddDriverPage(_databaseService, selectedDriver));
                DriversList.SelectedItem = null;
            }
        }
        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Driver driver)
            {
                bool confirm = await DisplayAlert("Подтверждение", $"Удалить водителя '{driver.FullName}'?", "Да", "Нет");
                if (confirm)
                {
                    try
                    {
                        await _databaseService.GetConnection().DeleteAsync(driver);
                        await DisplayAlert("Успех", "Водитель удалён.", "OK");
                        LoadDriversAsync();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Ошибка", $"Не удалось удалить водителя: {ex.Message}", "OK");
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
