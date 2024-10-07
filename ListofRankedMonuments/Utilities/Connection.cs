using System.Data.SqlClient;
using System.Net;

public class Connection
{
    private const string DatabaseName = "DB_QuanLyVanHoa";
    private const string UserId = "The Debuggers";
    private const string Password = "ifyouwanttoconnectyouneedtobecomeaprofessionalprogrammer";
    private const string ServerName = "DESKTOP-8I9KRD3"; // Cập nhật tên máy chủ

    public string GetConnectionString()
    {
        string ip = GetServerIp(ServerName);
        return $"Server={ip};Database={DatabaseName};User Id={UserId};Password={Password};";
    }

    private string GetServerIp(string serverName)
    {
        try
        {
            var hostEntry = Dns.GetHostEntry(serverName);
            return hostEntry.AddressList[0].ToString(); // Lấy địa chỉ IP đầu tiên
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting IP for {serverName}: {ex.Message}");
            return "127.0.0.1"; // Hoặc IP mặc định nào đó
        }
    }

    public void ConnectToDatabase()
    {
        string connectionString = GetConnectionString();
        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connection successful!");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }
        }
    }
}
