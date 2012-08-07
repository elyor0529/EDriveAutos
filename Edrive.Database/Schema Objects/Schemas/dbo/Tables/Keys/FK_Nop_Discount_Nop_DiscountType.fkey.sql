ALTER TABLE [dbo].[Nop_Discount]
    ADD CONSTRAINT [FK_Nop_Discount_Nop_DiscountType] FOREIGN KEY ([DiscountTypeID]) REFERENCES [dbo].[Nop_DiscountType] ([DiscountTypeID]) ON DELETE CASCADE ON UPDATE CASCADE;

