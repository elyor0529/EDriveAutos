ALTER TABLE [dbo].[Nop_Product]
    ADD CONSTRAINT [DF_Nop_Product_ProductTypeID] DEFAULT ((1)) FOR [ProductTypeID];

