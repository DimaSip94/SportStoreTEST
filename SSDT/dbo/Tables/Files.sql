CREATE TABLE [dbo].[Files]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[path] nvarchar(max) not null,
	CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED ([Id] ASC)
)
