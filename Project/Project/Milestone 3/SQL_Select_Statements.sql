USE [CPTS451PROJECT]
GO
-- QUERY 1
SELECT Business.b_id, Business.name, Business.latitude, Business.longitude FROM Business, Category WHERE Business.b_id=Category.b_id AND Category.name=''
GO

-- QUERY 2.i
SELECT Business.b_id, Business.name, Category.name as 'cname', AVG(Reviews.stars) as 'rating' FROM Business, Category, Reviews WHERE Business.b_id=Category.b_id AND Business.b_id=Reviews.b_id AND DATEPART(mm,Reviews.date)=6 AND DATEPART(yyyy, Reviews.date)=2011 GROUP BY Business.b_id, Business.name, Category.name
GO

-- QUERY 2.ii
SELECT Business.b_id, Business.name, AVG(Reviews.stars) as 'rating' FROM Business, Category, Reviews WHERE Business.b_id=Category.b_id AND Business.b_id=Reviews.b_id AND Category.name='Food' AND DATEPART(mm,Reviews.date)=1 AND DATEPART(yyyy, Reviews.date)=2012 GROUP BY Business.b_id, Business.name
GO
SELECT Business.b_id, Business.name, AVG(Reviews.stars) as 'rating' FROM Business, Category, Reviews WHERE Business.b_id=Category.b_id AND Business.b_id=Reviews.b_id AND Category.name='Food' AND DATEPART(mm,Reviews.date)=12 AND DATEPART(yyyy, Reviews.date)=2012 GROUP BY Business.b_id, Business.name
GO

-- QUERY 3
SELECT Business.b_id, Business.name, Business.stars, CheckIn.time, CheckIn.count FROM Business, Category, CheckIn WHERE Business.b_id=Category.b_id AND Business.b_id=CheckIn.b_id AND Category.name='' ORDER BY CheckIn.count DESC
GO

-- QUERY 4
SELECT Users.u_id as 'u_id', Users.name as 'name', Users.stars as 'stars', Category.name as 'cname', Count(Distinct Category.name) as 'count' FROM Users, Reviews, Category WHERE Users.u_id=Reviews.u_id AND Reviews.b_id=Category.b_id AND Users.u_id='' GROUP BY Users.u_id, Users.name, Users.stars, Category.name
GO

-- QUERY 5
SELECT Business.b_id, Business.latitude, Business.longitude, Business.stars, Category.name as 'cname' FROM Business, Category WHERE Business.b_id=Category.b_id
GO