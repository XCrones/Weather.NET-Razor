using System.ComponentModel.DataAnnotations.Schema;
using Weather.Domain.Interfaces;

namespace Weather.Domain.Entities
{
    [Table("ForecastUsers")]
    public class ForecastUser : ITiming, IUId, IId
    {
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("UId")]
        [ForeignKey("User")]
        public int UId { get; set; }

        [Column("Cities", TypeName = "jsonb")]
        public List<CityWeather> Cities { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

        [Column("DeleteAt")]
        public DateTime? DeleteAt { get; set; }

        public virtual User User { get; set; }
    }

    public class CityWeather
    {
        public int id { get; set; }

        public string name { get; set; }

        public string country { get; set; }
    }
}
