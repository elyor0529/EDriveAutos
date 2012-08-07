


CREATE PROCEDURE [dbo].[Nop_ShoppingCartItemInsert]
(
	@ShoppingCartItemID int = NULL output,
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
	INSERT
	INTO [Nop_ShoppingCartItem]
	(
		ShoppingCartTypeID,
		CustomerSessionGUID,
		ProductVariantID,
		AttributesXML,
		CustomerEnteredPrice,
		Quantity,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@ShoppingCartTypeID,
		@CustomerSessionGUID,
		@ProductVariantID,
		@AttributesXML,
		@CustomerEnteredPrice,
		@Quantity,
		@CreatedOn,
		@UpdatedOn
	)

	set @ShoppingCartItemID=SCOPE_IDENTITY()
END
