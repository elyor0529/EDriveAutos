ALTER TABLE [dbo].[Nop_Discount]
    ADD CONSTRAINT [FK_Nop_Discount_Nop_DiscountLimitation] FOREIGN KEY ([DiscountLimitationID]) REFERENCES [dbo].[Nop_DiscountLimitation] ([DiscountLimitationID]) ON DELETE CASCADE ON UPDATE CASCADE;

