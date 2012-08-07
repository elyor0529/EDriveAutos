CREATE TABLE [dbo].[ED_SystemParameter] (
    [ParameterID]   INT           IDENTITY (1, 1) NOT NULL,
    [ParameterName] NVARCHAR (50) NULL,
    [ParameterFrom] NVARCHAR (50) NOT NULL,
    [ParameterTo]   NVARCHAR (50) NOT NULL,
    [DisplayOrder]  INT           NOT NULL,
    [IsActive]      BIT           NULL,
    [CreatedOn]     DATETIME      NULL,
    [UpdatedOn]     DATETIME      NULL
);

