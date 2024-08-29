using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data.SqlClient;

public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly int _requiredPermission;
    private readonly string _functionName;

    public CustomAuthorizeAttribute(int requiredPermission, string functionName)
    {
        _requiredPermission = requiredPermission;
        _functionName = functionName;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userName = context.HttpContext.User.Identity.Name;
        var userPermissions = GetUserPermissions(userName, _functionName);

        if ((userPermissions & _requiredPermission) != _requiredPermission)
        {
            context.Result = new ForbidResult();
        }
    }

    private int GetUserPermissions(string userName, string functionName)
    {
        int permissions = 0;
        //string connectionString = "Server=YPKCXKLQ\\SQLEXPRESS;Database=QuanLyVanHoa;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";
        string connectionString = "Server=192.168.100.108;Database=QuanLyVanHoa;User Id=InternGo;Password=InternGo;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=False";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("FIG_GetUserPermissions", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@FunctionName", functionName);

            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                permissions = Convert.ToInt32(result);
            }
        }
        return permissions;
    }
}