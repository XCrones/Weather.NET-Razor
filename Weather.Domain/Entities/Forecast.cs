using System.ComponentModel.DataAnnotations.Schema;
using Weather.Domain.Interfaces;

namespace Weather.Domain.Entities
{
    [Table("Forecasts")]
    public class Forecast : IId, ITiming
    {
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("list", TypeName = "jsonb")]
        public List<WeatherItem> list { get; set; }

        [Column("city", TypeName = "jsonb")]
        public CityForecast city { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

        [Column("DeleteAt")]
        public DateTime? DeleteAt { get; set; }
    }

    public class CityForecast
    {
        public int id { get; set; }

        public string name { get; set; }

        public Coord coord { get; set; }

        public string country { get; set; }

        public int population { get; set; }

        public int timezone { get; set; }

        public int sunrise { get; set; }

        public int sunset { get; set; }
    }


    public class Coord
    {
        public float lat { get; set; }

        public float lon { get; set; }
    }

    public class WeatherItem
    {
        public int dt { get; set; }

        public Main main { get; set; }

        public List<Weather> weather { get; set; }

        public Clouds? clouds { get; set; }

        public Wind? wind { get; set; }

        public Rain rain { get; set; }

        public Snow show { get; set; }

        public int visibility { get; set; }

        public float pop { get; set; }

        public Sys sys { get; set; }

        public string dt_txt { get; set; }
    }

    public class Main
    {
        public float temp { get; set; }

        public int humidity { get; set; }

        public int pressure { get; set; }

        public float temp_max { get; set; }

        public float temp_min { get; set; }

        public int sea_level { get; set; }

        public float feels_like { get; set; }

        public int grnd_level { get; set; }

        public float temp_kf { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }

        public string main { get; set; }

        public string description { get; set; }

        public string icon { get; set; }
    }

    public class Clouds
    {
        public float all { get; set; }
    }

    public class Wind
    {
        public float? speed { get; set; }

        public float? deg { get; set; }

        public float? gust { get; set; }
    }

    public class Sys
    {
        public string pod { get; set; }
    }

    public class Snow
    {
        public float h1 { get; set; }

        public float h3 { get; set; }
    }

    public class Rain
    {
        public float h1 { get; set; }

        public float h3 { get; set; }
    }
}
