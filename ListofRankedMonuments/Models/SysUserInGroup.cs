using System.Text.Json.Serialization;
using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Models
{
    public class SysUserInGroup
    {
        [JsonPropertyName("UserInGroupID")]
        public int UserInGroupID { get; set; }

        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [JsonPropertyName("GroupID")]
        public int GroupID { get; set; }
    }

    public class SysUserInGroupCreateModel
    {
        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [JsonPropertyName("GroupID")]
        public int GroupID { get; set; }
    }

    public class SysUserInGroupUpdateModel
    {
        [JsonPropertyName("UserInGroupID")]
        public int UserInGroupID { get; set; }

        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [JsonPropertyName("GroupID")]
        public int GroupID { get; set; }
    }
}
