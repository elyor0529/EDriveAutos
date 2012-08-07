ALTER TABLE [dbo].[Nop_Order]
    ADD CONSTRAINT [FK_Nop_Order_Nop_PaymentStatus] FOREIGN KEY ([PaymentStatusID]) REFERENCES [dbo].[Nop_PaymentStatus] ([PaymentStatusID]) ON DELETE CASCADE ON UPDATE CASCADE;

