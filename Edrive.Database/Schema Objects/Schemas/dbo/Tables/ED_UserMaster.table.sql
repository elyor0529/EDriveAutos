CREATE TABLE [dbo].[ED_UserMaster] (
    [UserId]    INT           IDENTITY (1, 1) NOT NULL,
    [UserType]  TINYINT       NOT NULL,
    [UserName]  VARCHAR (50)  NOT NULL,
    [Password]  VARCHAR (50)  NOT NULL,
    [FirstName] VARCHAR (100) NOT NULL,
    [LastName]  VARCHAR (100) NULL,
    [IsActive]  BIT           NOT NULL,
    [CreatedOn] DATETIME      NULL,
    [CreatedBy] INT           NULL,
    [UpdatedBy] INT           NULL,
    [UpdatedOn] DATETIME      NULL
);

