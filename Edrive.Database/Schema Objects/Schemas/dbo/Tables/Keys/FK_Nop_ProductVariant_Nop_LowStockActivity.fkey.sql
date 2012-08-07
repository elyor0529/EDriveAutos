ALTER TABLE [dbo].[Nop_ProductVariant]
    ADD CONSTRAINT [FK_Nop_ProductVariant_Nop_LowStockActivity] FOREIGN KEY ([LowStockActivityID]) REFERENCES [dbo].[Nop_LowStockActivity] ([LowStockActivityID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

