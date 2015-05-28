-- Used on Microsoft SQL Server 2012.
use HW4
GO
-- QUESTION 1
-- 1.1.A
SELECT sName, Student.sID From Student, Enroll WHERE Enroll.sID=Student.sID AND Enroll.courseNo='CptS421' ORDER BY sName
GO
-- 1.1.B
SELECT Student.sID,courseNo  From Student, Enroll WHERE Enroll.sID=Student.sID AND Enroll.courseNo=SOME(SELECT preCourseNo FROM Prereq WHERE courseNo='CptS421')
GO
-- 1.2
SELECT sName From Student, Enroll WHERE Enroll.sID=Student.sID AND Enroll.courseNo='CptS421' AND NOT Enroll.sID=SOME(SELECT sID FROM Prereq, Enroll WHERE Enroll.courseNo=SOME(SELECT preCourseNo FROM Prereq WHERE courseNo='CptS421'))
GO
-- 1.3
SELECT courseNo From Course WHERE NOT EXISTS (SELECT Course.courseNo FROM Prereq WHERE Course.courseNo=Prereq.courseNo)
GO
-- 1.4
SELECT courseNo, AVG(grade) as 'Average Grade' From Enroll WHERE Enroll.courseNo=SOME(SELECT courseNo FROM Course WHERE dept='CHE') GROUP BY courseNo
GO
-- 1.5
CREATE VIEW MINGRADE AS SELECT Student.sID, Student.sName, Student.dept, Min(grade) as Grade FROM Enroll, Student WHERE Enroll.sID=Student.sID GROUP BY Student.sID, Student.sName, Student.dept
GO
SELECT MINGRADE.sName, MINGRADE.dept, Enroll.grade FROM Enroll, MINGRADE Where MINGRADE.sID=Enroll.sID AND Enroll.courseNo='CptS223' AND Enroll.grade=MINGRADE.grade
GO
-- 1.6
CREATE VIEW MECOURSES AS SELECT courseNo FROM Course WHERE dept='ME'
GO
CREATE VIEW MECOUNT AS SELECT Count(courseNo) AS c_count FROM MECOURSES
GO
CREATE VIEW MESTUDENTS AS SELECT sID, sNAME FROM Student WHERE dept='ME'
GO
CREATE VIEW MESTUDENTSCOURSES AS SELECT MESTUDENTS.sID, sName, Count(Enroll.courseNo) AS sCount FROM MESTUDENTS, Enroll WHERE Enroll.sID=MESTUDENTS.sID AND Enroll.courseNo=ANY(SELECT * FROM MECOURSES) GROUP BY MESTUDENTS.sID, sName
GO
CREATE VIEW MECROSS AS SELECT MESTUDENTS.sID, Enroll.courseNo FROM MESTUDENTS, Enroll CROSS JOIN MECOURSES WHERE MESTUDENTS.sID=Enroll.sID AND Enroll.courseNo=MECOURSES.courseNo
GO
SELECT MESTUDENTSCOURSES.sName, MESTUDENTSCOURSES.sID, MECROSS.courseNo FROM MECROSS, MECOUNT, MESTUDENTSCOURSES WHERE MECROSS.sID=MESTUDENTSCOURSES.sID AND MECOUNT.c_count=MESTUDENTSCOURSES.sCount
GO
-- 1.7
SELECT Student.sName, Student.dept FROM Student, Enroll, Course WHERE Student.sID=Enroll.sID AND Enroll.courseNo=Course.courseNo GROUP BY Student.sName, Student.dept HAVING COUNT(Course.dept)>3
GO
-- 1.Extra Credit

-- QUESTION 2
-- 2.1.i
-- R1 returns those students that have been (or are currently) enrolled in both CptS223 and CptS451, 
-- while the following statement returns the names of the students, and their grades in CptS223 and CptS451, for those that got a better grade in CptS451 than they did in CptS223.

-- 2.1.ii
CREATE VIEW CPTS223 AS SELECT Student.sName, Student.sID, Enroll.grade FROM Student, Enroll WHERE Enroll.courseNo='CptS223' AND Student.sID=Enroll.sID
GO
CREATE VIEW CPTS451 AS SELECT Student.sName, Student.sID, Enroll.grade FROM Student, Enroll WHERE Enroll.courseNo='CptS451' AND Student.sID=Enroll.sID
GO
SELECT CPTS223.sName, CPTS223.grade as 'CptS223 Grade', CPTS451.grade as 'CptS451 Grade' FROM CPTS223, CPTS451 WHERE CPTS223.sID=CPTS451.sID
GO

-- 2.2.i
-- This returns the names and departments of students who have less than a 3.0 GPA.

-- 2.2.ii
CREATE VIEW GPA AS SELECT Student.sName, Student.sID, Student.dept, AVG(Enroll.grade) as grade FROM Student, Enroll WHERE Student.sID=Enroll.sID GROUP BY Student.sName, Student.sID, Student.dept
GO
Select GPA.sName, GPA.dept FROM GPA WHERE grade<3.0
GO

-- QUESTION 3
-- 3.a: INSERT
-- The view is not updateable for insertions, since the selection does not contain information
-- for grade, which cannot be null and does not have a default value.

-- 3.b: UPDATE
-- The view is updateable for updates provided that any updates to courseNo and sID exist
-- in the Course and Student tables.

-- 3.c: DELETE
-- The view is updateable for deletions, since the selection does have enough information
-- to delete all tuples that can potentially produce the deleted view tuples.