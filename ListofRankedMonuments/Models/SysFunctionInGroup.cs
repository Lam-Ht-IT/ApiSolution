using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class SysFunctionInGroup
    {
        [JsonPropertyName("FunctionInGroupID")]
        public int FunctionInGroupID { get; set; }

        [JsonPropertyName("FunctionID")]
        public int FunctionID { get; set; }

        [JsonPropertyName("GroupID")]
        public int GroupID { get; set; }

        [JsonPropertyName("Permission")]
        public int Permission { get; set; }
    }

    public class SysFunctionInGroupInsertModel
    {
        [JsonPropertyName("FunctionID")]
        public int FunctionID { get; set; }

        [JsonPropertyName("GroupID")]
        public int GroupID { get; set; }

        [JsonPropertyName("Permission")]
        public int Permission { get; set; }
    }

    public class SysFunctionInGroupUpdateModel
    {
        [JsonPropertyName("FunctionInGroupID")]
        public int FunctionInGroupID { get; set; }

        [JsonPropertyName("FunctionID")]
        public int FunctionID { get; set; }

        [JsonPropertyName("GroupID")]
        public int GroupID { get; set; }

        [JsonPropertyName("Permission")]
        public int Permission { get; set; }
    }
}
