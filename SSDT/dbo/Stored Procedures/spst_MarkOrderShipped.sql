CREATE PROCEDURE [dbo].[spst_MarkOrderShipped]
	@orderID int,
	@shipped bit
AS
BEGIN
	update Orders set Shipped = @shipped where OrderID = @orderID
END
