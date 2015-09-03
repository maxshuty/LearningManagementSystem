using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampLMS.Models;
using Dapper;

namespace BootcampLMS.Data.Repositories
{
    public class AdminSearchRepo
    {
        public List<AdminSearchResult> SearchResults(string lastName, string firstName, string email, string roleName)
        {
            IEnumerable<AdminSearchResult> myResults = new List<AdminSearchResult>();

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                myResults =
                    conn.Query<AdminSearchResult>("dbo.GetSearchResults", new { LastName = lastName, FirstName = firstName, Email = email, RoleName = roleName },
                        commandType: CommandType.StoredProcedure);
            }
            return myResults.ToList();
        }
    }
}
