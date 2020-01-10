CREATE PROCEDURE [dbo].[spst_GetProducts]
	@page int = 1,
	@pageSize int = 10000,
	@category nvarchar(256) = '',
	@productID int = 0,
	@total int output
AS
BEGIN
	
	IF OBJECT_ID (N'tempdb..#getProducts', N'U') IS not NULL drop table #getProducts
	select *, LogoPath = isnull(f.path,'') into #getProducts 
	from Products as p 
	left join Files as f on f.Id = p.LogoID
	where (@category = '' or @category is null  or p.Category = @category)
	and (@productID = 0 or p.ProductID = @productID)
	and CartLineID is null
	order by p.ProductID 

	select @total = count(*) from #getProducts

	select *
	from #getProducts as p
	order by p.ProductID
	offset (@page-1) * @pageSize rows
	fetch next @pageSize rows only;

	IF OBJECT_ID (N'tempdb..#getProducts', N'U') IS not NULL drop table #getProducts
END
