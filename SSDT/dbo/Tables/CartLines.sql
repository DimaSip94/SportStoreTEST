CREATE TABLE [dbo].[CartLines]
(
	[CartLineID] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Quantity] INT NOT NULL, 
    [OrderID] INT NULL, 
    CONSTRAINT [FK_CartLines_Orders] FOREIGN KEY ([OrderID]) REFERENCES [Orders]([OrderID]) 

)
