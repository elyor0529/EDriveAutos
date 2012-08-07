ALTER TABLE [dbo].[Nop_ProductVariant]
    ADD CONSTRAINT [DF_Nop_ProductVariant_AllowOutOfStockOrders] DEFAULT ((0)) FOR [AllowOutOfStockOrders];

