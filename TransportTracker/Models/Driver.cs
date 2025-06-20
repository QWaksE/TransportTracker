using SQLite;

namespace TransportTracker.Models
{
    [Table("Drivers")]
    public class Driver
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(50)]
        public string LastName { get; set; }
        [NotNull, MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string MiddleName { get; set; }
        [NotNull]
        public int Experience { get; set; }
        public string FullName => $"{LastName} {FirstName} {(string.IsNullOrEmpty(MiddleName) ? "" : MiddleName)}".Trim();
    }
}
