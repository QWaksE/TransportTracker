using TransportTracker.Models;
using TransportTracker.Services;

namespace TransportTracker
{
    public partial class AddWorkPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private readonly Work? _work;
        private List<Route>? _routes;
        private List<Driver>? _drivers;

        public AddWorkPage(DatabaseService databaseService, Work? work = null)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _work = work;

            if (_work != null)
            {
                Title = "������������� ������";
                DepartureDatePicker.Date = _work.DepartureDate;
                ReturnDatePicker.Date = _work.ReturnDate;
                BonusEntry.Text = _work.Bonus.ToString();
            }
            else
            {
                Title = "�������� ������";
                DepartureDatePicker.Date = DateTime.Today;
                ReturnDatePicker.Date = DateTime.Today;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPickersAsync();
        }

        private async Task LoadPickersAsync()
        {
            try
            {
                var connection = _databaseService.GetConnection();
                _routes = await connection.Table<Route>().ToListAsync();
                _drivers = await connection.Table<Driver>().ToListAsync();

                RoutePicker.ItemsSource = _routes;
                RoutePicker.ItemDisplayBinding = new Binding("Name");
                DriverPicker.ItemsSource = _drivers;
                DriverPicker.ItemDisplayBinding = new Binding("FullName");
                SecondDriverPicker.ItemsSource = _drivers;
                SecondDriverPicker.ItemDisplayBinding = new Binding("FullName");

                if (_work != null)
                {
                    // �������� �� null �� �����, ��� ��� _routes � _drivers ��� ����������������
                    RoutePicker.SelectedItem = _routes!.FirstOrDefault(r => r.Id == _work.RouteId);
                    DriverPicker.SelectedItem = _drivers!.FirstOrDefault(d => d.Id == _work.DriverId);
                    SecondDriverPicker.SelectedItem = _work.SecondDriverId.HasValue
                        ? _drivers!.FirstOrDefault(d => d.Id == _work.SecondDriverId.Value)
                        : null;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("������", $"�� ������� ��������� ������: {ex.Message}", "OK");
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (RoutePicker.SelectedItem == null || DriverPicker.SelectedItem == null)
                {
                    await DisplayAlert("������", "�������� ������� � ������� ��������.", "OK");
                    return;
                }

                if (DepartureDatePicker.Date > ReturnDatePicker.Date)
                {
                    await DisplayAlert("������", "���� ����������� �� ����� ���� ������ ���� ��������.", "OK");
                    return;
                }

                decimal bonus = 0;
                if (!string.IsNullOrWhiteSpace(BonusEntry.Text) && !decimal.TryParse(BonusEntry.Text, out bonus))
                {
                    await DisplayAlert("������", "��������� ������ ������.", "OK");
                    return;
                }

                var work = _work ?? new Work();
                work.RouteId = ((Route)RoutePicker.SelectedItem).Id; // ���������� ����������, ��� ��� ��������� �� null
                work.DriverId = ((Driver)DriverPicker.SelectedItem).Id; // ���������� ����������
                work.SecondDriverId = SecondDriverPicker.SelectedItem != null
                    ? ((Driver)SecondDriverPicker.SelectedItem).Id
                    : null;
                work.DepartureDate = DepartureDatePicker.Date;
                work.ReturnDate = ReturnDatePicker.Date;
                work.Bonus = bonus;

                var connection = _databaseService.GetConnection();
                if (_work == null)
                    await connection.InsertAsync(work);
                else
                    await connection.UpdateAsync(work);

                await DisplayAlert("�����", _work == null ? "������ ���������." : "������ ���������.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("������", $"�� ������� ��������� ������: {ex.Message}", "OK");
            }
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}