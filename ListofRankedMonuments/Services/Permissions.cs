namespace QUANLYVANHOA.Services
{
    public static class Permissions
    {
        public const int View = 1;     // 0001
        public const int Add = 2;      // 0010
        public const int Edit = 4;     // 0100
        public const int Delete = 8;   // 1000

        // Tổ hợp quyền
        public const int FullControl = View | Add | Edit | Delete; // 1111
    }

}
