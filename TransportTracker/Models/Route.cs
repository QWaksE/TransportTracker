using SQLite;

namespace TransportTracker.Models
{
    [Table("Routes")]
    public class Route
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(100)]
        public string Name { get; set; }
        [NotNull]
        public double Distance { get; set; }
        [NotNull]
        public int DaysInTransit { get; set; }
        [NotNull]
        public decimal Payment { get; set; }
    }
}