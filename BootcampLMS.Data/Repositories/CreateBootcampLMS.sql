USE Master

go

DROP DATABASE BootcampLMS

go

CREATE DATABASE BootcampLMS

go

USE BootcampLMS
go

go

CREATE TABLE UserProfile
(
UserId nvarchar(128) NOT NULL PRIMARY KEY,
FirstName nvarchar(100),
LastName nvarchar(100) NOT NULL,
RequestedRole nvarchar(100) NOT NULL,
GradeLevel int
);

CREATE TABLE ParentStudent
(
ParentId nvarchar(128) NOT NULL,
StudentId nvarchar(128) NOT NULL,
CONSTRAINT PK_ParentStudent PRIMARY KEY (ParentId, StudentID)
)

CREATE TABLE Roster
(
RosterId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
UserId nvarchar(128) NOT NULL,
CourseId int NOT NULL,
IsActive bit DEFAULT 1
)

CREATE TABLE Course
(
CourseId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
TeacherId nvarchar(128) NOT NULL,
Name nvarchar(100) NOT NULL,
Department nvarchar(100),
CourseDescription nvarchar(MAX),
StartDate datetime2(0),
EndDate datetime2(0),
GradeLevel int,
IsArchived bit NOT NULL
)

CREATE TABLE Assignment
(
AssignmentId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
CourseId int NOT NULL,
Name nvarchar(100),
AssignmentDescription nvarchar(MAX),
DueDate datetime2(0),
PointsPossible int NOT NULL
)

CREATE TABLE AssignmentTracker
(
Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
AssignmentId int NOT NULL,
RosterId int NOT NULL,
EarnedPoints int
)

ALTER TABLE ParentStudent
ADD CONSTRAINT FK_ParentStudent_UserProfile
FOREIGN KEY (ParentId) References UserProfile (UserId)

ALTER TABLE ParentStudent
ADD CONSTRAINT FK_ParentStudent_UserProfile2
FOREIGN KEY (StudentId) References UserProfile (UserId)

ALTER TABLE Roster
ADD CONSTRAINT FK_Roster_UserProfile
FOREIGN KEY (UserId) References UserProfile (UserId)

ALTER TABLE Roster
ADD CONSTRAINT FK_Roster_Course
FOREIGN KEY (CourseId) References Course (CourseId)

ALTER TABLE Assignment
ADD CONSTRAINT FK_Assignment_Course
FOREIGN KEY (CourseId) References Course (CourseId)

ALTER TABLE AssignmentTracker
ADD CONSTRAINT FK_AssignmentTracker_Assignment
FOREIGN KEY (AssignmentId) References Assignment (AssignmentId)

ALTER TABLE AssignmentTracker
ADD CONSTRAINT FK_AssignmentTracker_Roster
FOREIGN KEY (RosterId) References Roster (RosterId)

ALTER TABLE Course
ADD CONSTRAINT FK_Course_UserProfile
FOREIGN KEY (TeacherId) References UserProfile (UserId)

GO

CREATE PROCEDURE [dbo].[GetKids]
	@ParentId nvarchar(MAX)

AS

SELECT * FROM ParentStudent PS
INNER JOIN UserProfile P
ON PS.StudentId = P.UserId
WHERE PS.ParentId = @ParentId

GO

USE BootcampLMS
GO

CREATE PROCEDURE [dbo].[GetCourseGrades]
	@UserId nvarchar(MAX)

AS

SELECT C.CourseId, C.Name AS CourseName, SUM(A.PointsPossible) AS PointsPossible, SUM(AT.EarnedPoints) AS EarnedPoints
FROM Assignment A
INNER JOIN AssignmentTracker AT
ON A.AssignmentId = AT.AssignmentId
INNER JOIN Roster R
ON AT.RosterId = R.RosterId
INNER JOIN Course C
ON R.CourseId = C.CourseId
WHERE R.UserId = @UserId
GROUP BY C.CourseId, C.Name

GO

CREATE PROCEDURE [dbo].[GetRosterTableItem]
	@CourseId int

AS

SELECT U.FirstName, U.LastName, A.Email, R.RosterId, C.CourseId FROM Course C
INNER JOIN Roster R
ON C.CourseId = R.CourseId
INNER JOIN UserProfile U
ON R.UserId = U.UserId
INNER JOIN AspNetUsers A
ON U.UserId = A.Id
WHERE C.CourseId = @CourseId
AND R.IsActive = 1

GO

USE BootcampLMS

GO

CREATE PROCEDURE [dbo].[GetAssignments]
	@CourseId int,
	@UserId nvarchar(100)

AS

SELECT R.UserId, A.Name AS AssignmentName, AT.EarnedPoints AS EarnedPoints, A.PointsPossible AS PointsPossible FROM AssignmentTracker AT
INNER JOIN Assignment A
ON AT.AssignmentId = A.AssignmentId
INNER JOIN Roster R
ON AT.RosterId = R.RosterId
WHERE R.CourseId = @CourseId AND R.UserId = @UserId

GO

USE BootcampLMS

GO

CREATE PROCEDURE [dbo].[GetGrades]
	@CourseId int

AS

SELECT r.RosterId, SUM(t.EarnedPoints) AS EarnedPointsTotal, SUM(a.PointsPossible) AS PointsPossible
FROM Roster R
	INNER JOIN UserProfile u on r.UserId = u.UserId
	INNER JOIN Course c on r.CourseId = c.CourseId
	INNER JOIN AssignmentTracker t on r.RosterId = t.RosterId
	INNER JOIN Assignment a on t.AssignmentId = a.AssignmentId

WHERE c.CourseId = @CourseId
GROUP BY r.RosterId

GO

CREATE PROCEDURE [dbo].[GetStudents]
	@CourseId int

AS

SELECT P.UserId FROM Course C
INNER JOIN Roster R
ON C.CourseId = R.CourseId
INNER JOIN UserProfile P
ON R.UserId = P.UserId
WHERE C.CourseId = @CourseId

GO

CREATE PROCEDURE [dbo].[GetUnassignedUsers]
		
AS

SELECT LastName, FirstName, Email, RequestedRole, u.UserId
FROM UserProfile u
	INNER JOIN AspNetUsers a ON u.UserId = a.Id
	LEFT JOIN AspNetUserRoles ar ON a.Id = ar.UserId
WHERE ar.UserId IS NULL

GO

CREATE PROCEDURE [dbo].[GetUserDetails]
	@UserId nvarchar(128)

AS

SELECT LastName, FirstName, Email
FROM UserProfile u
	INNER JOIN AspNetUsers a ON u.UserId = a.Id
WHERE a.Id = @UserId

GO

CREATE PROCEDURE [dbo].[GetUserRoles]
	@UserId nvarchar(128)

AS

SELECT Name
FROM AspNetUserRoles ar
	INNER JOIN AspNetRoles a ON ar.RoleId = a.Id
WHERE ar.UserId = @UserId

GO

CREATE PROCEDURE [dbo].[GetTeachers]

AS

SELECT UP.UserId, UP.FirstName, UP.LastName FROM UserProfile UP
INNER JOIN AspNetUserRoles UR
ON UP.UserId = UR.UserId
INNER JOIN AspNetRoles R
ON UR.RoleId = R.Id
WHERE R.Name = 'Teacher'

GO

CREATE PROCEDURE [dbo].[GetSearchResults]
		@FirstName nvarchar(100),
		@LastName nvarchar(100),
		@Email nvarchar(256),
		@RoleName nvarchar(256)

AS

SELECT DISTINCT u.UserId, u.LastName, u.FirstName, a.Email
FROM UserProfile u
	INNER JOIN AspNetUserRoles ar ON u.UserId = ar.UserId
	INNER JOIN AspNetUsers a ON u.UserId = a.Id
	INNER JOIN AspNetRoles anr ON ar.RoleId = anr.Id
WHERE (@LastName IS NULL OR u.LastName LIKE '%' + @LastName + '%')
	AND (@FirstName IS NULL OR u.FirstName LIKE '%' + @FirstName + '%')
	AND (@Email IS NULL OR a.Email LIKE '%' + @Email + '%')
	AND (@RoleName IS NULL OR anr.Name LIKE '%' + @RoleName + '%')
	
GO

CREATE PROCEDURE [dbo].[GetStudentAndGrade]
			@CourseId int

AS

SELECT u.FirstName, u.LastName, at.EarnedPoints, a.PointsPossible, u.UserId
FROM UserProfile u
	INNER JOIN Roster r ON r.UserId = u.UserId
	INNER JOIN Assignment a ON r.CourseId = a.CourseId
	INNER JOIN AssignmentTracker at ON at.Id = a.AssignmentId
WHERE u.RequestedRole = 'Student' AND a.CourseId = @CourseId

GO

CREATE PROCEDURE [dbo].[GetAssignmentName]
			@CourseId int

AS

SELECT Name, a.AssignmentId
FROM Assignment a
WHERE a.CourseId = @CourseId
ORDER BY DueDate

GO

CREATE PROCEDURE [dbo].[GetEarnedPoints]
			@CourseId int

AS

SELECT EarnedPoints 
FROM AssignmentTracker at
	INNER JOIN Assignment a ON at.AssignmentId = a.AssignmentId
WHERE a.CourseId = @CourseId
ORDER BY a.DueDate

GO

USE BootcampLMS

GO

CREATE PROCEDURE [dbo].[RosterFlipActive]
	@RosterId int

AS

UPDATE Roster SET
IsActive = IsActive ^ 1
WHERE RosterId = @RosterId

GO

USE BootcampLMS

GO

CREATE PROCEDURE [dbo].[GetStudentsNotInCourse]
	@CourseId int

AS

SELECT DISTINCT U.FirstName, U.LastName, U.GradeLevel, U.UserId AS StudentId FROM UserProfile U
LEFT JOIN Roster R
ON U.UserId = R.UserId
WHERE U.UserId NOT IN
(
SELECT R.UserId FROM UserProfile U
INNER JOIN Roster R
ON U.UserId = R.UserId
WHERE R.CourseId = @CourseId
AND R.IsActive = 1
)