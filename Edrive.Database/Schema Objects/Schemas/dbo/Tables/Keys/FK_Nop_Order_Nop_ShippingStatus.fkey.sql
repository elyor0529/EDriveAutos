ALTER TABLE [dbo].[Nop_Order]
    ADD CONSTRAINT [FK_Nop_Order_Nop_ShippingStatus] FOREIGN KEY ([ShippingStatusID]) REFERENCES [dbo].[Nop_ShippingStatus] ([ShippingStatusID]) ON DELETE CASCADE ON UPDATE CASCADE;

