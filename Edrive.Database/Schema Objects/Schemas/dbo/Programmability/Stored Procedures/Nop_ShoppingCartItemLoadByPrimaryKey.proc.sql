

CREATE PROCEDURE [dbo].[Nop_ShoppingCartItemLoadByPrimaryKey]
(
	@ShoppingCartItemID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShoppingCartItem]
	WHERE
		ShoppingCartItemID = @ShoppingCartItemID
END
