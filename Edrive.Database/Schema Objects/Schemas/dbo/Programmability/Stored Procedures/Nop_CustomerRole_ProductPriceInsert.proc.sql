

CREATE PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceInsert]
(
	@CustomerRoleProductPriceID int = NULL output,
	@CustomerRoleID int,
	@ProductVariantID int,
	@Price money
)
AS
BEGIN
	INSERT
	INTO [Nop_CustomerRole_ProductPrice]
	(
		[CustomerRoleID],
		[ProductVariantID],
		[Price]
	)
	VALUES
	(
		@CustomerRoleID,
		@ProductVariantID,
		@Price
	)

	set @CustomerRoleProductPriceID=SCOPE_IDENTITY()
END
