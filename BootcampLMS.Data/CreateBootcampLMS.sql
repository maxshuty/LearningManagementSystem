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
IsActive bit NOT NULL
)

CREATE TABLE Course
(
CourseId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
TeacherId nvarchar(128) NOT NULL,
Name nvarchar(100) NOT NULL,
Department nvarchar(100),
CourseDescription nvarchar(max),
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
AssignmentDescription nvarchar(max),
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
	@ParentId nvarchar(max)

AS

SELECT * FROM ParentStudent PS
INNER JOIN UserProfile P
ON PS.StudentId = P.UserId
WHERE PS.ParentId = @ParentId

GO

USE BootcampLMS
GO

CREATE PROCEDURE [dbo].[GetCourseGrades]
	@UserId nvarchar(max)

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

SELECT U.FirstName, U.LastName, A.Email FROM Course C
INNER JOIN Roster R
ON C.CourseId = R.CourseId
INNER JOIN UserProfile U
ON R.UserId = U.UserId
INNER JOIN AspNetUsers A
ON U.UserId = A.Id
WHERE C.CourseId = @CourseId

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