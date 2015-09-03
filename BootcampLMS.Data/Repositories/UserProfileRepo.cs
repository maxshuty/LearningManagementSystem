
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
    public class UserProfileRepo
    {
        public void Delete(string id)
        {
            string sql = @"DELETE FROM UserProfile
                            WHERE UserId = @Id";

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue(@"Id", id);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void Add(UserProfile myUserProfile)
        {
            string sql = @"INSERT INTO UserProfile (UserId, FirstName, LastName, RequestedRole, GradeLevel)
                                VALUES (@UserId, @FirstName, @LastName, @RequestedRole, @GradeLevel)";

            var conn = new SqlConnection(Settings.ConnectionString);

            using (conn)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@UserId", myUserProfile.UserId);
                cmd.Parameters.AddWithValue("@FirstName", myUserProfile.FirstName);
                cmd.Parameters.AddWithValue("@LastName", myUserProfile.LastName);
                cmd.Parameters.AddWithValue("@RequestedRole", myUserProfile.RequestedRole);
                cmd.Parameters.AddWithValue("@GradeLevel", ((object)myUserProfile.GradeLevel ?? DBNull.Value));

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public UserProfile GetById(string id)
        {
            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"Select *
                                       FROM UserProfile
                                       WHERE UserId = @UserId";

                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@UserId", id);

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

        public List<UserProfile> GetAll()
        {

            List<UserProfile> myList = new List<UserProfile>();

            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = new SqlCommand();

                cmd.CommandText = @"Select *
                                       FROM UserProfile";

                cmd.Connection = cn;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        myList.Add(PopulateFromDataReader(dr));
                    }
                }
            }
            return myList;
        }

        public void Edit(UserProfile myUserProfile)
        {
            using (SqlConnection cn = new SqlConnection(Settings.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE UserProfile
                                    SET UserId = @UserId,
                                    FirstName = @FirstName,
                                    LastName = @LastName,
                                    RequestedRole = @RequestedRole,
                                    GradeLevel = @GradeLevel
                                    WHERE UserId = @UserId";

                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@UserId", myUserProfile.UserId);
                cmd.Parameters.AddWithValue("@FirstName", myUserProfile.FirstName);
                cmd.Parameters.AddWithValue("@LastName", myUserProfile.LastName);
                cmd.Parameters.AddWithValue("@RequestedRole", myUserProfile.RequestedRole);
                cmd.Parameters.AddWithValue("@GradeLevel", ((object)myUserProfile.GradeLevel ?? DBNull.Value));

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        private UserProfile PopulateFromDataReader(SqlDataReader dr)
        {
            UserProfile myUserProfile = new UserProfile();
            myUserProfile.UserId = (string)dr["UserId"];
            myUserProfile.FirstName = (string)dr["FirstName"];
            myUserProfile.LastName = (string)dr["LastName"];
            myUserProfile.RequestedRole = (string)dr["RequestedRole"];
            if (dr["GradeLevel"] != DBNull.Value)
                myUserProfile.GradeLevel = (int)dr["GradeLevel"];

            return myUserProfile;
        }

        public List<UnassignedUser> GetUnassignedUsers()
        {
            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                return
                    conn.Query<UnassignedUser>("dbo.GetUnassignedUsers",
                        commandType: CommandType.StoredProcedure).ToList();
            }

        }

        public List<UserProfile> GetTeachers()
        {
            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                return
                    conn.Query<UserProfile>("dbo.GetTeachers",
                        commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
