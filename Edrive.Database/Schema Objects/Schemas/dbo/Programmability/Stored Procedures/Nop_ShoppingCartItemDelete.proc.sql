

CREATE PROCEDURE [dbo].[Nop_ShoppingCartItemDelete]
(
	@ShoppingCartItemID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ShoppingCartItem]
	WHERE
		[ShoppingCartItemID] = @ShoppingCartItemID
END
