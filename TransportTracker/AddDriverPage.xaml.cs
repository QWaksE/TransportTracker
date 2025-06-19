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
                Title = "������������� ��������";
                LastNameEntry.Text = _driver.LastName;
                FirstNameEntry.Text = _driver.FirstName;
                MiddleNameEntry.Text = _driver.MiddleName;
                ExperienceEntry.Text = _driver.Experience.ToString();
            }
            else
            {
                Title = "�������� ��������";
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
                    await DisplayAlert("������", "��������� ������������ ���� (�������, ���, ����).", "OK");
                    return;
                }
                if (!int.TryParse(ExperienceEntry.Text, out int experience))
                {
                    await DisplayAlert("������", "��������� ������ ����� (������ ���� ����� �����).", "OK");
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
                await DisplayAlert("�����", _driver == null ? "�������� ��������." : "�������� �������.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("������", $"�� ������� ��������� ��������: {ex.Message}", "OK");
            }
        }
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}