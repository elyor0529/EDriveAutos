CREATE TABLE [dbo].[ED_DealersFaq] (
    [FaqId]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [OrderId]   BIGINT         NOT NULL,
    [Question]  NVARCHAR (MAX) NOT NULL,
    [Answer]    NVARCHAR (MAX) NOT NULL,
    [IsActive]  BIT            NOT NULL,
    [CreatedOn] DATETIME       NOT NULL,
    [UpdatedOn] DATETIME       NULL
);

