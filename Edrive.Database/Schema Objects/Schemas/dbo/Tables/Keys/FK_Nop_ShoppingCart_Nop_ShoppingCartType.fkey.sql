ALTER TABLE [dbo].[Nop_ShoppingCartItem]
    ADD CONSTRAINT [FK_Nop_ShoppingCart_Nop_ShoppingCartType] FOREIGN KEY ([ShoppingCartTypeID]) REFERENCES [dbo].[Nop_ShoppingCartType] ([ShoppingCartTypeID]) ON DELETE CASCADE ON UPDATE CASCADE;

