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
    public class ParentStudentRepo
    {
        public void Add(string parentId, string studentId)
        {
            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                string sql = "INSERT INTO ParentStudent (ParentId, StudentId) VALUES (@ParentId, @StudentId)";
                conn.Execute(sql, new {ParentId = parentId, StudentId = studentId});
            }
        }

        public List<ParentStudent> GetAll()
        {
            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                string sql = "SELECT * FROM ParentStudent";
                return conn.Query<ParentStudent>(sql).ToList();
            }
        }

        public List<UserProfile> GetKidsByParent(string parentId)
        {
            IEnumerable<UserProfile> myKids = new List<UserProfile>();

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                myKids =
                    conn.Query<UserProfile>("dbo.GetKids", new {ParentId = parentId},
                        commandType: CommandType.StoredProcedure);
            }

            return myKids.ToList();
        }

        public void Delete(string parentId, string studentId)
        {
            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                string sql = "DELETE FROM ParentStudent WHERE ParentId = @ParentId AND StudentId = @StudentId";
                conn.Execute(sql, new {ParentId = parentId, StudentId = studentId});
            }
        }
    }
}
