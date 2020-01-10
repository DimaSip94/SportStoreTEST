CREATE PROCEDURE [dbo].[spst_GetCategories]
	 
AS
BEGIN
	select distinct p.Category 
	from Products as p
	order by p.Category
END
