namespace QUANLYVANHOA.Models
{
        public class SysFunction
        {
            public int FunctionID { get; set; }
            public string FunctionName { get; set; }
            public string Description { get; set; }
        }

    public class SysFunctionInsertModel
    {
        public string FunctionName { get; set; }
        public string Description { get; set; }
    }

    public class SysFunctionUpdateModel
    {
        public int FunctionID { get; set; }
        public string FunctionName { get; set; }
        public string Description { get; set; }
    }
}
