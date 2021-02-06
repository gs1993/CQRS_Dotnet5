
CREATE VIEW [dbo].[StudentsView]
WITH SCHEMABINDING
AS

SELECT
	s.Id as Id, 
	s.[Name], 
	s.Email,
	c1.Course1,
	c1.Course1Grade,
	c1.Course1Credits,
	c2.Course2,
	c2.Course2Grade,
	c2.Course2Credits
FROM dbo.Students s
OUTER APPLY  (
		SELECT TOP 1
			e.StudentID,
			e.Id,
			c.[Name] as Course1,
			c.Credits as Course1Credits,
			e.Grade as Course1Grade
		FROM dbo.Enrollments e
		JOIN dbo.Courses c ON c.Id = e.CourseID
		WHERE s.Id = e.StudentID
		ORDER BY c.Id
	 ) c1
OUTER APPLY  (
		SELECT
			e.StudentID,
			e.Id,
			c.[Name] as Course2,
			c.Credits as Course2Credits,
			e.Grade as Course2Grade
		FROM dbo.Enrollments e
		JOIN dbo.Courses c ON c.Id = e.CourseID
		WHERE s.Id = e.StudentID
		ORDER BY c.Id
		OFFSET 1 ROWS FETCH NEXT 1 ROWS ONLY
	 ) c2

GO
