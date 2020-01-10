CREATE PROCEDURE [dbo].[spst_DeleteProduct]
	@productID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			if(@productID>0)
			begin	
				if(exists(
					select top 1 1 from Products as p 
					join CartLines as cl on cl.CartLineID = p.CartLineID
					join Orders as o on o.OrderID = cl.OrderID 
					where p.ProductID = @productID
				))
				begin
					RAISERROR(N'This product in the order!',16,1)
				end
				declare @LogoPath nvarchar(max) = (select top 1 isnull(f.path,'') from Products as p join Files as f on f.Id = p.LogoID where p.ProductID = @productID)
				delete CartLines where CartLineID = (select top 1 CartLineID from Products where ProductID = @productID)
				delete Products where ProductID = @productID
				if(@LogoPath<>'')
				begin
					delete Files where path = @LogoPath
				end
				select @LogoPath
			end
			else
			begin
				RAISERROR(N'This product doesn''t exist!',16,1)
			end
		COMMIT
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK
		DECLARE @Err varchar(512)
		SET @Err= ERROR_MESSAGE()
		RAISERROR (@Err, 16, 1)
	END CATCH
END
