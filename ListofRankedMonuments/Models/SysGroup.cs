namespace QUANLYVANHOA.Models
{
    public class SysGroup
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
    }
    public class SysGroupInsertModel
    {
        public string GroupName { get; set; }
        public string Description { get; set; }
    }
    public class SysGroupUpdateModel
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
    }
}
