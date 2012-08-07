

CREATE PROCEDURE [dbo].[Nop_ShoppingCartItemLoadByCustomerSessionGUID]
(
	@ShoppingCartTypeID int,
	@CustomerSessionGUID uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShoppingCartItem]
	WHERE ShoppingCartTypeID=@ShoppingCartTypeID and CustomerSessionGUID=@CustomerSessionGUID 
	order by CreatedOn
END
