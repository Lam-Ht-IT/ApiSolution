using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Models
{
    public class SysUserInGroup
    {
        public int UserInGroupID { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
    }

    public class SysUserInGroupCreateModel
    {
        public int UserID { get; set; }
        public int GroupID { get; set; }
    }

    public class SysUserInGroupUpdateModel
    {
        public int UserInGroupID { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
    }
}
