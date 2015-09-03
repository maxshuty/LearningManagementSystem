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
    public class CourseRepo
    {
        public void Delete(int id)
        {
            string sql = @"DELETE FROM Course
                            WHERE CourseId = @CourseId";

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue(@"CourseId", id);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void Add(Course myCourse)
        {
            string sql = @"INSERT INTO Course (TeacherId, Name, Department, CourseDescription, StartDate, EndDate, GradeLevel, IsArchived)
                                VALUES (@TeacherId, @Name, @Department, @CourseDescription, @StartDate, @EndDate, @GradeLevel, @IsArchived)
                           SELECT CAST(SCOPE_IDENTITY() AS int)";

            var conn = new SqlConnection(Settings.ConnectionString);

            using (conn)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@TeacherId", myCourse.TeacherId);
                cmd.Parameters.AddWithValue("@Name", myCourse.Name);
                cmd.Parameters.AddWithValue("@Department", myCourse.Department);
                cmd.Parameters.AddWithValue("@CourseDescription", myCourse.CourseDescription);
                cmd.Parameters.AddWithValue("@StartDate", myCourse.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", myCourse.EndDate);
                cmd.Parameters.AddWithValue("@GradeLevel", myCourse.GradeLevel);
                cmd.Parameters.AddWithValue("@IsArchived", myCourse.IsArchived);

                conn.Open();
                var wtf = cmd.ExecuteScalar();
                int intwtf = (int)wtf;

                myCourse.CourseId = intwtf;
            }
        }

        public Course GetById(int id)
        {
            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"Select *
                                       FROM Course
                                       WHERE CourseId = @CourseId";

                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@CourseId", id);

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

        public List<Course> GetAll()
        {
            List<Course> myCourses = new List<Course>();

            using (var cn = new SqlConnection(Settings.ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"Select *
                                       FROM Course";

                cmd.Connection = cn;
                
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        myCourses.Add(PopulateFromDataReader(dr));
                    }
                }
            }
            return myCourses;
        }

        public void Edit(Course myCourse)
        {
            using (SqlConnection cn = new SqlConnection(Settings.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Course
                                    SET TeacherId = @TeacherId,
                                    Name = @Name,
                                    CourseDescription = @CourseDescription,
                                    Department = @Department,
                                    EndDate = @EndDate,
                                    StartDate = @StartDate,
                                    GradeLevel = @GradeLevel,
                                    IsArchived = @IsArchived
                                    WHERE CourseId = @CourseId";

                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@TeacherId", myCourse.TeacherId);
                cmd.Parameters.AddWithValue("@Name", myCourse.Name);
                cmd.Parameters.AddWithValue("@CourseDescription", myCourse.CourseDescription);
                cmd.Parameters.AddWithValue("@Department", myCourse.Department);
                cmd.Parameters.AddWithValue("@EndDate", myCourse.EndDate);
                cmd.Parameters.AddWithValue("@StartDate", myCourse.StartDate);
                cmd.Parameters.AddWithValue("@GradeLevel", myCourse.GradeLevel);
                cmd.Parameters.AddWithValue("@IsArchived", myCourse.IsArchived);
                cmd.Parameters.AddWithValue("@CourseId", myCourse.CourseId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        private Course PopulateFromDataReader(SqlDataReader dr)
        {
            Course course = new Course();
            course.CourseId = (int)dr["CourseId"];
            course.TeacherId = dr["TeacherId"].ToString();
            course.Name = dr["Name"].ToString();
            course.Department = dr["Department"].ToString();
            course.CourseDescription = dr["CourseDescription"].ToString();

            if (dr["StartDate"] != DBNull.Value)
                course.StartDate = (DateTime)dr["StartDate"];
            if (dr["EndDate"] != DBNull.Value)
                course.EndDate = (DateTime)dr["EndDate"];
            if (dr["GradeLevel"] != DBNull.Value)
                course.GradeLevel = (int)dr["GradeLevel"];

            course.IsArchived = (bool) dr["IsArchived"];

            return course;
        }

        public List<CourseSummary> GetSummariesByTeacher(string teacherId)
        {
            string sql = @"SELECT C.CourseId, C.Name, C.IsArchived, COUNT(R.RosterId) AS StudentCount
                            FROM Course C
                                LEFT JOIN Roster R ON C.CourseId = R.CourseId
                            WHERE TeacherId = @TeacherId
                            GROUP BY C.CourseId, C.Name, C.IsArchived";

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                return conn.Query<CourseSummary>(sql, new {TeacherId = teacherId}).ToList();
            }
        }

        public List<AnalyticsModelItem> GetGrades(int courseid)
        {
            IEnumerable<AnalyticsModelItem> myAnal = new List<AnalyticsModelItem>();

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                myAnal =
                    conn.Query<AnalyticsModelItem>("dbo.GetGrades", new { CourseId = courseid },
                        commandType: CommandType.StoredProcedure);
            }

            return myAnal.ToList();
        }

        public List<string> GetStudents(int courseid)
        {
            IEnumerable<string> students = new List<string>();

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                students =
                    conn.Query<string>("dbo.GetStudents", new { CourseId = courseid },
                        commandType: CommandType.StoredProcedure);
            }

            return students.ToList();
        }

        public List<StudentSearchResult> GetStudentsNotInCourse(int courseid)
        {
            IEnumerable<StudentSearchResult> students = new List<StudentSearchResult>();

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                students =
                    conn.Query<StudentSearchResult>("dbo.GetStudentsNotInCourse", new { CourseId = courseid },
                        commandType: CommandType.StoredProcedure);
            }

            return students.ToList();
        }
    }
}
