using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace BootcampLMS.Data.Repositories
{
    public class IdentityRepo
    {
        public void Add(string roleId, string userId)
        {
            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                string sql = @"IF NOT EXISTS
                                ( SELECT * FROM AspNetUserRoles WHERE RoleId = @RoleId AND UserId = @UserId)
                                BEGIN
	                                INSERT INTO AspNetUserRoles (RoleId, UserId) VALUES (@RoleId, @UserId)
                                END;";

                conn.Execute(sql, new { RoleId = roleId, UserId = userId });
            }
        }

        public void Delete(string roleId, string userId)
        {
            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                string sql = "DELETE FROM AspNetUserRoles WHERE EXISTS ( SELECT * FROM AspNetUserRoles WHERE RoleId = @RoleId AND UserId = @UserId)";
                conn.Execute(sql, new { RoleId = roleId, UserId = userId });
            }
        }
    }
}
