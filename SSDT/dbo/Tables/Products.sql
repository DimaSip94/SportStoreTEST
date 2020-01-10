CREATE TABLE [dbo].[Products] (
    [ProductID]   INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (32)   NOT NULL,
    [Description] NVARCHAR (128)  NULL,
    [Price]       DECIMAL (18, 2) NOT NULL,
    [Category]    NVARCHAR (128)  NULL,
	[CartLineID] INt NULL,
    [LogoID] INT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([ProductID] ASC),
	CONSTRAINT [FK_Products_CartLines] FOREIGN KEY ([CartLineID]) REFERENCES [CartLines]([CartLineID]) ON DELETE CASCADE, 
    CONSTRAINT [CK_Products_Name] UNIQUE ([Name]),
    CONSTRAINT [FK_Products_Files] FOREIGN KEY ([LogoID]) REFERENCES [Files]([Id]) 
);

