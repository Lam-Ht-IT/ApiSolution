namespace QUANLYVANHOA.Models
{
    public class SysFunctionInGroup
    {
        public int FunctionInGroupID { get; set; }
        public int FunctionID { get; set; }
        public int GroupID { get; set; }
        public int Permission { get; set; }
    }

    public class SysFunctionInGroupInsertModel
    {
        public int FunctionID { get; set; }
        public int GroupID { get; set; }
        public int Permission { get; set; }
    }

    public class SysFunctionInGroupUpdateModel
    {
        public int FunctionInGroupID { get; set; }
        public int FunctionID { get; set; }
        public int GroupID { get; set; }
        public int Permission { get; set; }
    }
}
