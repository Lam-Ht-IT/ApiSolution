namespace QUANLYVANHOA.Models
{
        public class SysFunction
        {
            public int FunctionID { get; set; }
            public string FunctionName { get; set; }
            public string Description { get; set; }
        }

    public class SysFunctionModelInsert
    {
        public string FunctionName { get; set; }
        public string Description { get; set; }
    }

    public class SysFunctionModelUpdate
    {
        public int FunctionID { get; set; }
        public string FunctionName { get; set; }
        public string Description { get; set; }
    }
}
