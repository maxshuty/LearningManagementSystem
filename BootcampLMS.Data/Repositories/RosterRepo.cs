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
    public class RosterRepo
    {
        public void Delete(int id)
        {
            string sql = @"DELETE FROM Roster
                            WHERE RosterId = @RosterId";

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue(@"RosterId", id);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void Add(Roster myRoster)
        {
            string sql = @"INSERT INTO Roster (UserId, CourseId, IsActive)
                                VALUES (@UserId, @CourseId, @IsActive)
                           SELECT CAST(SCOPE_IDENTITY() AS int)";

            var conn = new SqlConnection(Settings.ConnectionString);

            using (conn)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@UserId", myRoster.UserId);
                cmd.Parameters.AddWithValue("@CourseId", myRoster.CourseId);
                cmd.Parameters.AddWithValue("@IsActive", myRoster.IsActive);

                conn.Open();

                myRoster.RosterId = (int)cmd.ExecuteScalar();
            }
        }

        public Roster GetById(int id)
        {
            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"Select *
                                       FROM Roster
                                       WHERE RosterId = @RosterId";

                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@RosterId", id);

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

        public List<Roster> GetAll()
        {
            List<Roster> myRosterList = new List<Roster>();

            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"Select *
                                       FROM Roster";

                cmd.Connection = cn;
                
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        myRosterList.Add(PopulateFromDataReader(dr));
                    }
                }
            }
            return myRosterList;
        }

        public void Edit(Roster myRoster)
        {
            using (SqlConnection cn = new SqlConnection(Settings.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Roster
                                    SET UserId = @UserId,
                                    CourseId = @CourseId,
                                    IsActive = @IsActive
                                    WHERE RosterId = @RosterId";

                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@UserId", myRoster.UserId);
                cmd.Parameters.AddWithValue("@CourseId", myRoster.CourseId);
                cmd.Parameters.AddWithValue("@RosterId", myRoster.RosterId);
                cmd.Parameters.AddWithValue("@IsActive", myRoster.IsActive);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        private Roster PopulateFromDataReader(SqlDataReader dr)
        {
            Roster myRoster = new Roster();
            myRoster.UserId = (string)dr["UserId"];
            myRoster.CourseId = (int)dr["CourseId"];
            myRoster.IsActive = (bool)dr["IsActive"];
            myRoster.RosterId = (int)dr["RosterId"];

            return myRoster;
        }

        public List<RosterTableViewModel> GetRosterTableItem(int courseid)
        {
            IEnumerable<RosterTableViewModel> myRosterTableItems = new List<RosterTableViewModel>();

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                myRosterTableItems =
                    conn.Query<RosterTableViewModel>("dbo.GetRosterTableItem", new { CourseId = courseid },
                        commandType: CommandType.StoredProcedure);
            }

            return myRosterTableItems.ToList();
        }


        public void Archive(int rosterId)
        {
            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                conn.Execute("dbo.RosterFlipActive", new {RosterId = rosterId}, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
