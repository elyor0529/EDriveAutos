ALTER TABLE [dbo].[Nop_Country]
    ADD CONSTRAINT [DF_Nop_Country_Published] DEFAULT ((1)) FOR [Published];

