using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;
using BootcampLMS.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace BootcampLMS.UI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BootcampLMS.UI.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
             //Don't want to redo this multiple times, only if we drop the database and need to re-seed
            if (new UserProfileRepo().GetAll().Any())
                return;

            CreateRole(context, "Admin");
            CreateRole(context, "Parent");
            CreateRole(context, "Student");
            CreateRole(context, "Teacher");

            CreateUser(context, "admin@test.com", "password123", "Admin", "Admin", "Admin", null);
            string fakeTeacherId = CreateUser(context, "teacher@test.com", "password123", "Test", "Teacher", "Teacher", null);
            string fakeStudentId = CreateUser(context, "student@test.com", "password123", "Test", "Student", "Student", null);
            string fakeParentId = CreateUser(context, "parent@test.com", "password123", "Test", "Parent", "Parent", null);

            

            CourseRepo myCourseRepo = new CourseRepo();
            Course myCourse = new Course();

            myCourse.Name = "Algebra";
            myCourse.Department = "Math";
            myCourse.CourseDescription = "This is the one with letters instead of numbers.";
            myCourse.StartDate = new DateTime(2011, 11, 12);
            myCourse.EndDate = new DateTime(2011, 12, 10);
            myCourse.GradeLevel = 5;
            myCourse.TeacherId = fakeTeacherId;
            myCourseRepo.Add(myCourse);

            Course myCourse2 = new Course();

            myCourse2.Name = "English";
            myCourse2.Department = "Language";
            myCourse2.CourseDescription = "This is the one with words that say things.";
            myCourse2.StartDate = new DateTime(2011, 10, 12);
            myCourse2.EndDate = new DateTime(2011, 12, 14);
            myCourse2.GradeLevel = 7;
            myCourse2.TeacherId = fakeTeacherId;
            myCourseRepo.Add(myCourse2);

            RosterRepo myRosterRepo = new RosterRepo();
            Roster myRoster = new Roster();
            Roster myRoster2 = new Roster();

            myRoster.CourseId = myCourse.CourseId;
            myRoster.UserId = fakeStudentId;
            myRosterRepo.Add(myRoster);

            myRoster2.CourseId = myCourse2.CourseId;
            myRoster2.UserId = fakeStudentId;
            myRosterRepo.Add(myRoster2);

            Random myRandom = new Random();

            for (int i = 0; i < 100; i++)
            {
                string fakeStudentLoopId = CreateUser(context, "student" + i + "@test.com", "password123", "Test" + i, "Student" + i, "Student", myRandom.Next(1,12));
                Roster myRosterLoop = new Roster();
                myRosterLoop.CourseId = myCourse.CourseId;
                myRosterLoop.UserId = fakeStudentLoopId;
                myRosterRepo.Add(myRosterLoop);

                if (i%2 == 0)
                {
                    Roster myRosterLoop2 = new Roster();
                    myRosterLoop2.CourseId = myCourse2.CourseId;
                    myRosterLoop2.UserId = fakeStudentLoopId;
                    myRosterRepo.Add(myRosterLoop2);
                }
            }

          

            for (int i = 1; i <= 20; i++)
            {
                AssignmentRepo myAssignmentRepo = new AssignmentRepo();
                Assignment myAssignment= new Assignment();
                
                myAssignment.Name = "Homework" + i;
                myAssignment.AssignmentDescription = "All the equations, solve them.";
                myAssignment.CourseId = myCourse.CourseId;
                myAssignment.DueDate = new DateTime(2011 + i%4, myRandom.Next(11) + 1, myRandom.Next(27) + 1);
                myAssignment.PointsPossible = myRandom.Next(50, 200);
                myAssignmentRepo.Add(myAssignment);

                AssignmentTrackerRepo myTrackerRepo = new AssignmentTrackerRepo();
                AssignmentTracker myTracker = new AssignmentTracker();

                myTracker.AssignmentId = myAssignment.AssignmentId;
                for (int j = 1; j < 100; j++)
                {
                    myTracker.RosterId = j;
                    myTracker.EarnedPoints = myRandom.Next((int)(myAssignment.PointsPossible * 0.55), myAssignment.PointsPossible);
                    myTrackerRepo.Add(myTracker);
                }
            }

            ParentStudentRepo myParentStudentRepo = new ParentStudentRepo();

            myParentStudentRepo.Add(fakeParentId, fakeStudentId);

        }

        private void CreateRole(ApplicationDbContext context, string role)
        {
            using (var roleStore = new RoleStore<IdentityRole>(context))
            {
                using (var roleManager = new RoleManager<IdentityRole>(roleStore))
                {
                    roleManager.Create(new IdentityRole { Name = role });
                }
            }
        }

        private string CreateUser(ApplicationDbContext context, string email, string password, string firstName, string lastName, string reqRole, int? gradeLevel)
        {
            // We already have the context, the EntityFramework object that talks to the database tables

            // First, the "store" that Microsoft puts in the middle. Don't ask me why.
            using (var userStore = new UserStore<ApplicationUser>(context))
            {
                // Next the manager, that actually does useful things. Like create user, add user to role, etc.
                using (var userMgr = new UserManager<ApplicationUser>(userStore))
                {
                    // This is the stub object, the bare minimum for an ASP.NET "User"
                    ApplicationUser user = new ApplicationUser
                    {
                        Email = email,
                        UserName = email
                    };

                    // Hey EntityFramework, please create the AspNetUsers row, and figure out the user id
                    userMgr.Create(user, password);
                    userMgr.AddToRole(user.Id, reqRole);

                    // Now that we have the UserId, we can create our UserProfile record and insert it with our repository
                    UserProfile profile = new UserProfile
                    {
                        UserId = user.Id,
                        FirstName = firstName,
                        LastName = lastName,
                        RequestedRole = reqRole,
                        GradeLevel = gradeLevel
                    };
                    UserProfileRepo repo = new UserProfileRepo();
                    repo.Add(profile);

                    // Now finally, return the user id so we can create Courses, etc.
                    return user.Id;
                }
            }
        }
    }
}

