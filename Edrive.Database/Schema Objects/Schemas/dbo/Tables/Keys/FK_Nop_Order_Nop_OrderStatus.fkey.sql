ALTER TABLE [dbo].[Nop_Order]
    ADD CONSTRAINT [FK_Nop_Order_Nop_OrderStatus] FOREIGN KEY ([OrderStatusID]) REFERENCES [dbo].[Nop_OrderStatus] ([OrderStatusID]) ON DELETE CASCADE ON UPDATE CASCADE;

