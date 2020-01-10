CREATE PROCEDURE [dbo].[spst_GetOrders]
	@page int = 1,
	@pageSize int = 10000,
	@shipped bit = 0
AS
BEGIN
	
	IF OBJECT_ID (N'tempdb..#getOrders', N'U') IS not NULL drop table #getOrders
	select * into #getOrders 
	from Orders as o 
	where (@shipped is null or isnull(o.Shipped,0) = @shipped)
	order by o.OrderID 

	select total = count(*) from #getOrders

	select *
	from #getOrders as o
	order by o.OrderID
	offset (@page-1) * @pageSize rows
	fetch next @pageSize rows only;

	--возвращаем данные по CartLines
	IF OBJECT_ID (N'tempdb..#getCartLines', N'U') IS not NULL drop table #getCartLines
	select * into #getCartLines
	from CartLines as cl
	where cl.OrderID in (select OrderID from #getOrders)

	select * 
	from #getCartLines as cl
	inner join Products as p on p.CartLineID = cl.CartLineID

	--возвращаем данные по Продуктам
	select * from Products where CartLineID in (select CartLineID from #getCartLines)

	IF OBJECT_ID (N'tempdb..#getOrders', N'U') IS not NULL drop table #getOrders
	IF OBJECT_ID (N'tempdb..#getCartLines', N'U') IS not NULL drop table #getCartLines
END
