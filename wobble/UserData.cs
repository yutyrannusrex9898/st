using SQLite;

namespace wobble
{
    public class UserData
    {
        [PrimaryKey, Column("_device_id")]
        public string DeviceId { get; set; }

        [Column("_kills")]
        public int Kills { get; set; }

        [Column("_deaths")]
        public int Deaths { get; set; }
    }
}
