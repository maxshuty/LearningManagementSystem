using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampLMS.Models;

namespace BootcampLMS.Data.Repositories
{
    public class AssignmentRepo 
    {
        //Random Change
        public void Delete(int id)
        {
            string sql = @"DELETE FROM Assignment
                            WHERE AssignmentId = @Id";

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue(@"Id", id);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void Add(Assignment myAss)
        {
            string sql = @"INSERT INTO Assignment (CourseId, Name, AssignmentDescription, DueDate, PointsPossible)
                                VALUES (@CourseId, @Name, @AssignmentDescription, @DueDate, @PointsPossible)
                           SELECT CAST(SCOPE_IDENTITY() AS INT)";

            var conn = new SqlConnection(Settings.ConnectionString);

            using (conn)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@CourseId", myAss.CourseId);
                cmd.Parameters.AddWithValue("@Name", myAss.Name);
                cmd.Parameters.AddWithValue("@AssignmentDescription", myAss.AssignmentDescription);
                cmd.Parameters.AddWithValue("@DueDate", myAss.DueDate);
                cmd.Parameters.AddWithValue("@PointsPossible", myAss.PointsPossible);

                conn.Open();

                myAss.AssignmentId = (int)cmd.ExecuteScalar();
            }
        }

        public Assignment GetById(int id)
        {
            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"Select *
                                       FROM Assignment
                                       WHERE AssignmentId = @AssId";

                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@Assid", id);

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

        public void Edit(Assignment myAss)
        {
            using (SqlConnection cn = new SqlConnection(Settings.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Assignment
                                    SET CourseId = @CourseId,
                                    Name = @Name,
                                    AssignmentDescription = @AssignmentDescription,
                                    DueDate = @DueDate,
                                    PointsPossible = @PointsPossible
                                    WHERE AssignmentId = @AssignmentId";

                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@CourseId", myAss.CourseId);
                cmd.Parameters.AddWithValue("@Name", myAss.Name);
                cmd.Parameters.AddWithValue("@AssignmentDescription", myAss.AssignmentDescription);
                cmd.Parameters.AddWithValue("@DueDate", myAss.DueDate);
                cmd.Parameters.AddWithValue("@PointsPossible", myAss.PointsPossible);
                cmd.Parameters.AddWithValue("@AssignmentId", myAss.AssignmentId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        private Assignment PopulateFromDataReader(SqlDataReader dr)
        {
            Assignment myAss = new Assignment();
            myAss.AssignmentId = (int) dr["AssignmentId"];
            myAss.CourseId = (int) dr["CourseId"];
            myAss.Name = dr["Name"].ToString();
            myAss.AssignmentDescription = dr["AssignmentDescription"].ToString();

            if (dr["DueDate"] != DBNull.Value)
                myAss.DueDate = (DateTime) dr["DueDate"];

            myAss.PointsPossible = (int) dr["PointsPossible"];

            return myAss;
        }

        public List<Assignment> GetAll()
        {
            List<Assignment> myAssignments = new List<Assignment>();

            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"Select *
                                       FROM Assignment";

                cmd.Connection = cn;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        myAssignments.Add(PopulateFromDataReader(dr));
                    }
                }
            }
            return myAssignments;
        }


    }
}
