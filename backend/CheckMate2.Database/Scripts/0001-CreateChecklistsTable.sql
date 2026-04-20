CREATE TABLE [dbo].[Checklists] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(200) NOT NULL,
    CONSTRAINT [PK_Checklists] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE UNIQUE NONCLUSTERED INDEX [IX_Checklists_Name]
    ON [dbo].[Checklists] ([Name] ASC);
