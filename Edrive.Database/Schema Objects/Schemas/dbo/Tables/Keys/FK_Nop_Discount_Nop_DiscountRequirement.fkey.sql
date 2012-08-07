ALTER TABLE [dbo].[Nop_Discount]
    ADD CONSTRAINT [FK_Nop_Discount_Nop_DiscountRequirement] FOREIGN KEY ([DiscountRequirementID]) REFERENCES [dbo].[Nop_DiscountRequirement] ([DiscountRequirementID]) ON DELETE CASCADE ON UPDATE CASCADE;

