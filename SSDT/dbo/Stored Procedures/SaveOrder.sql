CREATE PROCEDURE [dbo].[SaveOrder]
	 @Name NVARCHAR(256) = null
    ,@Line1 NVARCHAR(256) = null
    ,@Line2 NVARCHAR(256) = null 
    ,@Line3 NVARCHAR(256) = null 
    ,@City NVARCHAR(256)= null
    ,@State NVARCHAR(256)= null
    ,@Zip NVARCHAR(50) = null 
    ,@Country NVARCHAR(128)= null
    ,@GiftWrap BIT = null
	,@CartLines [dbo].[CartLineType] readonly

AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			
			declare @NewOrderID int = 0
			declare @NewCartLineID int = 0

			if(exists(select top 1 1 from Orders where Name = @Name))
			begin
				RAISERROR(N'The order with same name is already exist',16,1)
			end

			if(not exists(select top 1 1 from @CartLines))
			begin
				RAISERROR(N'Your cart is empty',16,1)
			end

			insert into Orders
			values(@Name, @Line1, @Line2, @Line3, @City, @State, @Zip, @Country, @GiftWrap, 0)
			set @NewOrderID = SCOPE_IDENTITY()
			
			IF OBJECT_ID (N'tempdb..#CartLines', N'U') IS not NULL drop table #CartLines
			select * into #CartLines from @CartLines
			while ((select count(*) from #CartLines) > 0)
			begin
				declare @currentProductID int = 0
				select top 1 @currentProductID = ProductID from @CartLines
				insert into CartLines
				select top 1 Quantity, @NewOrderID from #CartLines where ProductID = @currentProductID
				set @NewCartLineID = SCOPE_IDENTITY()
				update Products set CartLineID = @NewCartLineID where ProductID = @currentProductID
				delete #CartLines where ProductID = @currentProductID 
			end

			IF OBJECT_ID (N'tempdb..#CartLines', N'U') IS not NULL drop table #CartLines

			select @NewOrderID

		COMMIT
	END TRY
	BEGIN CATCH
		IF OBJECT_ID (N'tempdb..#CartLines', N'U') IS not NULL drop table #CartLines
		IF @@TRANCOUNT > 0 ROLLBACK
		DECLARE @Err varchar(512)
		SET @Err= ERROR_MESSAGE()
		RAISERROR (@Err, 16, 1)
	END CATCH
END
