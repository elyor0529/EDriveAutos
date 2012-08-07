CREATE TABLE [dbo].[ED_Zipcodes] (
    [ZipID]     INT            IDENTITY (1, 1) NOT NULL,
    [zip_code]  NVARCHAR (MAX) NOT NULL,
    [latitude]  FLOAT          NOT NULL,
    [longitude] FLOAT          NOT NULL,
    [city]      VARCHAR (50)   NOT NULL,
    [state]     VARCHAR (50)   NOT NULL,
    [country]   VARCHAR (50)   NOT NULL,
    [zip_class] VARCHAR (50)   NOT NULL
);

