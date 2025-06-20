using SQLite;
using System;

namespace TransportTracker.Models
{
    [Table("Work")]
    public class Work
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int RouteId { get; set; }
        [NotNull]
        public int DriverId { get; set; }
        public int? SecondDriverId { get; set; }
        [NotNull]
        public DateTime DepartureDate { get; set; }
        [NotNull]
        public DateTime ReturnDate { get; set; }
        public decimal Bonus { get; set; }
        [Ignore]
        public string RouteName { get; set; }
        [Ignore]
        public string DriverFullName { get; set; }
        [Ignore]
        public string SecondDriverFullName { get; set; }
        [Ignore]
        public decimal RoutePayment { get; set; }
        [Ignore]
        public int DriverExperience { get; set; }
        [Ignore]
        public int? SecondDriverExperience { get; set; }
        [Ignore]
        public decimal TotalCost
        {
            get
            {
                // Расчёт оплаты для первого водителя: базовая оплата * (1 + 0.05 * стаж)
                decimal driver1Payment = RoutePayment * (1 + 0.05m * DriverExperience);
                // Расчёт оплаты для второго водителя, если он есть
                decimal driver2Payment = SecondDriverExperience.HasValue
                    ? RoutePayment * (1 + 0.05m * SecondDriverExperience.Value)
                    : 0;
                return driver1Payment + driver2Payment + Bonus;
            }
        }
    }
}
