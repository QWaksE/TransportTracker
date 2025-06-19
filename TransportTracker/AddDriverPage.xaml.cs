using TransportTracker.Models;
using TransportTracker.Services;

namespace TransportTracker
{
    public partial class AddDriverPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private readonly Driver? _driver;
        public AddDriverPage(DatabaseService databaseService, Driver? driver = null)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _driver = driver;
            if (_driver != null)
            {
                Title = "Редактировать водителя";
                LastNameEntry.Text = _driver.LastName;
                FirstNameEntry.Text = _driver.FirstName;
                MiddleNameEntry.Text = _driver.MiddleName;
                ExperienceEntry.Text = _driver.Experience.ToString();
            }
            else
            {
                Title = "Добавить водителя";
            }
        }
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
                    string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
                    string.IsNullOrWhiteSpace(ExperienceEntry.Text))
                {
                    await DisplayAlert("Ошибка", "Заполните обязательные поля (фамилия, имя, стаж).", "OK");
                    return;
                }
                if (!int.TryParse(ExperienceEntry.Text, out int experience))
                {
                    await DisplayAlert("Ошибка", "Проверьте формат стажа (должно быть целое число).", "OK");
                    return;
                }
                var driver = _driver ?? new Driver();
                driver.LastName = LastNameEntry.Text;
                driver.FirstName = FirstNameEntry.Text;
                driver.MiddleName = MiddleNameEntry.Text;
                driver.Experience = experience;
                var connection = _databaseService.GetConnection();
                if (_driver == null)
                    await connection.InsertAsync(driver);
                else
                    await connection.UpdateAsync(driver);
                await DisplayAlert("Успех", _driver == null ? "Водитель добавлен." : "Водитель обновлён.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось сохранить водителя: {ex.Message}", "OK");
            }
        }
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}