

CREATE PROCEDURE [dbo].[Nop_ShoppingCartItemUpdate]
(
	@ShoppingCartItemID int,
	@ShoppingCartTypeID int,
	@CustomerSessionGUID uniqueidentifier,
	@ProductVariantID int,
	@AttributesXML XML,
	@CustomerEnteredPrice money,
	@Quantity int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ShoppingCartItem]
	SET
			ShoppingCartTypeID=@ShoppingCartTypeID,
			CustomerSessionGUID=@CustomerSessionGUID,
			ProductVariantID=@ProductVariantID,	
			AttributesXML=@AttributesXML,
			CustomerEnteredPrice=@CustomerEnteredPrice,
			Quantity=@Quantity,
			CreatedOn=@CreatedOn,
			UpdatedOn=@UpdatedOn
	WHERE
		ShoppingCartItemID = @ShoppingCartItemID
END
