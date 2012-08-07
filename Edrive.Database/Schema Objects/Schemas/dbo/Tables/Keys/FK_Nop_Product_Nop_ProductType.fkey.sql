ALTER TABLE [dbo].[Nop_Product]
    ADD CONSTRAINT [FK_Nop_Product_Nop_ProductType] FOREIGN KEY ([ProductTypeID]) REFERENCES [dbo].[Nop_ProductType] ([ProductTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

