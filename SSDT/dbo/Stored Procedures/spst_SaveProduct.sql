CREATE PROCEDURE [dbo].[spst_SaveProduct]
	@productID int,
	@Name nvarchar(32),
	@Description nvarchar(128),
	@Category NVARCHAR (128),
	@Price    DECIMAL (18, 2),
	@LogoPath nvarchar(max) = null
AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			--сохранение файла
			declare @LogoID int = null
			if(isnull(@LogoPath,'')<>'')
			begin
				select top 1 @LogoID = Id from Files where path = @LogoPath
				if(isnull(@LogoID,0)=0)
				begin
					insert into Files (path) values (@LogoPath)
					select @LogoID = SCOPE_IDENTITY()
				end
			end
			
			if(@productID>0)
			begin	
				if(not exists(select top 1 1 from Products where ProductID = @productID))
				begin
					RAISERROR(N'This product doesn''t exist!',16,1)
				end
				update Products 
				set Category = @Category, Description = @Description, Name = @Name, Price = @Price, LogoID = @LogoID where ProductID = @productID
			end
			else
			begin
				if(exists(select top 1 1 from Products where Name = @Name))
				begin
					RAISERROR(N'The same name product already exist!',16,1)
				end
				insert into Products
				values(@Name, @Description, @Price, @Category, null, @LogoID)
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
