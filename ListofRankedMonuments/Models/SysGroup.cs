using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class SysGroup
    {
        [JsonPropertyName("GroupID")]
        public int GroupID { get; set; }

        [JsonPropertyName("GroupName")]
        public string GroupName { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
    public class SysGroupInsertModel
    {
        [JsonPropertyName("GroupName")]
        public string GroupName { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
    public class SysGroupUpdateModel
    {
        [JsonPropertyName("GroupID")]
        public int GroupID { get; set; }

        [JsonPropertyName("GroupName")]
        public string GroupName { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
}
