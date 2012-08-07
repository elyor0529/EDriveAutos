CREATE TABLE [dbo].[ED_InterestedCustomer] (
    [InterestedCustomerID] INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]            NVARCHAR (50)  NULL,
    [LastName]             NVARCHAR (50)  NULL,
    [ProductId]            INT            NULL,
    [Email]                NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (50)  NULL,
    [ZipCode]              NVARCHAR (50)  NULL,
    [ContactType]          INT            NULL,
    [InterestType]         INT            NULL,
    [AdditionalComment]    NVARCHAR (MAX) NULL,
    [IsActive]             BIT            NULL,
    [CreatedOn]            DATETIME       NULL,
    [UpdatedOn]            DATETIME       NULL,
    [Financing]            BIT            NULL,
    [Price_Lock]           BIT            NULL,
    [Trade_in]             BIT            NULL,
    [CustomerId]           INT            NULL
);

