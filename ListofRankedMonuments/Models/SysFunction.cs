using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class SysFunction
    {
        [JsonPropertyName("FunctionID")]
        public int FunctionID { get; set; }

        [JsonPropertyName("FunctionName")]
        public string FunctionName { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }

    public class SysFunctionInsertModel
    {
        [JsonPropertyName("FunctionName")]
        public string FunctionName { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }

    public class SysFunctionUpdateModel
    {
        [JsonPropertyName("FunctionID")]
        public int FunctionID { get; set; }

        [JsonPropertyName("FunctionName")]
        public string FunctionName { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
}
