using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampLMS.Models;
using BootcampLMS.UI.Models;
using Dapper;

namespace BootcampLMS.Data.Repositories
{
    public class AssignmentTrackerRepo
    {
         public void Delete(int id)
        {
            string sql = @"DELETE FROM AssignmentTracker
                            WHERE Id = @Id";

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue(@"Id", id);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void Add(AssignmentTracker myAss)
        {
            string sql = @"INSERT INTO AssignmentTracker (AssignmentId, RosterId, EarnedPoints)
                                VALUES (@AssignmentId, @RosterId, @EarnedPoints)
                           SELECT CAST(SCOPE_IDENTITY() AS INT)";

            var conn = new SqlConnection(Settings.ConnectionString);

            using (conn)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@AssignmentId", myAss.AssignmentId);
                cmd.Parameters.AddWithValue("@RosterId", myAss.RosterId);
                cmd.Parameters.AddWithValue("@EarnedPoints", myAss.EarnedPoints);
                
                conn.Open();

                myAss.Id = (int)cmd.ExecuteScalar();
            }
        }

        public AssignmentTracker GetById(int id)
        {
            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"Select *
                                       FROM AssignmentTracker
                                       WHERE Id = @id";

                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@id", id);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        return PopulateFromDataReader(dr);
                    }
                }
            }
            return null;
        }

        public List<AssignmentTracker> GetAll()
        {
            List<AssignmentTracker> myAssTrackingList = new List<AssignmentTracker>();

            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"Select *
                                       FROM AssignmentTracker";

                cmd.Connection = cn;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        myAssTrackingList.Add(PopulateFromDataReader(dr));
                    }
                }
            }
            return myAssTrackingList;
        }

        public void Edit(AssignmentTracker myAss)
        {
            using (SqlConnection cn = new SqlConnection(Settings.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE AssignmentTracker
                                    SET AssignmentId = @AssignmentId,
                                    RosterId = @RosterId,
                                    EarnedPoints = @EarnedPoints
                                    WHERE Id = @Id";

                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@AssignmentId", myAss.AssignmentId);
                cmd.Parameters.AddWithValue("@RosterId", myAss.RosterId);
                cmd.Parameters.AddWithValue("@EarnedPoints", myAss.EarnedPoints);
                cmd.Parameters.AddWithValue("@Id", myAss.Id);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        private AssignmentTracker PopulateFromDataReader(SqlDataReader dr)
        {
            AssignmentTracker myAss = new AssignmentTracker();
            myAss.Id = (int)dr["Id"];
            myAss.AssignmentId = (int)dr["AssignmentId"];
            myAss.RosterId = (int) dr["RosterId"];
            
            if (dr["EarnedPoints"] != DBNull.Value)
                myAss.EarnedPoints = (int)dr["EarnedPoints"];

            return myAss;
        }

        public List<StudentDashboardTableItem> GetCourseGrades(string userid)
        {
            IEnumerable<StudentDashboardTableItem> myTableVms = new List<StudentDashboardTableItem>();

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                myTableVms =
                    conn.Query<StudentDashboardTableItem>("dbo.GetCourseGrades", new { UserId = userid },
                        commandType: CommandType.StoredProcedure);
            }

            return myTableVms.ToList();
        }

        public List<AssignmentViewModel> GetAssignments(string userid, int courseid)
        {
            IEnumerable<AssignmentViewModel> myAsses = new List<AssignmentViewModel>();

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                myAsses =
                    conn.Query<AssignmentViewModel>("dbo.GetAssignments", new { UserId = userid, CourseId = courseid },
                        commandType: CommandType.StoredProcedure);
            }

            return myAsses.ToList();
        }
    }
}
