using Dapper;
using IWantApp.Endpoints.Employees;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Data;

public class QueryAllUsersWithClaimName
{
    private readonly IConfiguration configuration;

    public QueryAllUsersWithClaimName(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<IEnumerable<EmployeeResponse>> Execute(int page, int rows)
    {
        var db = new SqlConnection(configuration["ConnectionString:IWantApp"]);

        var query =
            @"select Email, ClaimValue as Name
            from AspNetUsers u
            inner join AspNetUser c on u.id = c.UserId and claimType = 'Name'
            order by Name
            offset (@page - 1) * @rows rows fetch next @rows rows only";

        return await db.QueryAsync<EmployeeResponse>(query, new { page, rows });
    }
}
