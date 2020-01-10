CREATE TABLE [dbo].[Orders]
(
	[OrderID] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(256) NOT NULL, 
    [Line1] NVARCHAR(256) NOT NULL, 
    [Line2] NVARCHAR(256) NULL, 
    [Line3] NVARCHAR(256) NULL, 
    [City] NVARCHAR(256) NOT NULL, 
    [State] NVARCHAR(256) NOT NULL, 
    [Zip] NVARCHAR(50) NULL, 
    [Country] NVARCHAR(128) NOT NULL, 
    [GiftWrap] BIT NULL, 
    [Shipped] BIT NULL
)
