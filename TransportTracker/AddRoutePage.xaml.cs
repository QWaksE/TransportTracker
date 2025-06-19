using TransportTracker.Models;
using TransportTracker.Services;

namespace TransportTracker
{
    public partial class AddRoutePage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private readonly Route? _route;
        public AddRoutePage(DatabaseService databaseService, Route? route = null)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _route = route;

            if (_route != null)
            {
                Title = "Редактировать маршрут";
                NameEntry.Text = _route.Name;
                DistanceEntry.Text = _route.Distance.ToString();
                DaysInTransitEntry.Text = _route.DaysInTransit.ToString();
                PaymentEntry.Text = _route.Payment.ToString();
            }
            else
            {
                Title = "Добавить маршрут";
            }
        }
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
                    string.IsNullOrWhiteSpace(DistanceEntry.Text) ||
                    string.IsNullOrWhiteSpace(DaysInTransitEntry.Text) ||
                    string.IsNullOrWhiteSpace(PaymentEntry.Text))
                {
                    await DisplayAlert("Ошибка", "Заполните все поля.", "OK");
                    return;
                }

                if (!double.TryParse(DistanceEntry.Text, out double distance) ||
                    !int.TryParse(DaysInTransitEntry.Text, out int daysInTransit) ||
                    !decimal.TryParse(PaymentEntry.Text, out decimal payment))
                {
                    await DisplayAlert("Ошибка", "Проверьте формат числовых полей.", "OK");
                    return;
                }
                var route = _route ?? new Route();
                route.Name = NameEntry.Text;
                route.Distance = distance;
                route.DaysInTransit = daysInTransit;
                route.Payment = payment;
                var connection = _databaseService.GetConnection();
                if (_route == null)
                    await connection.InsertAsync(route);
                else
                    await connection.UpdateAsync(route);

                await DisplayAlert("Успех", _route == null ? "Маршрут добавлен." : "Маршрут обновлён.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось сохранить маршрут: {ex.Message}", "OK");
            }
        }
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}