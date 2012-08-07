

CREATE PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceUpdate]
(
	@CustomerRoleProductPriceID int,
	@CustomerRoleID int,
	@ProductVariantID int,
	@Price money
)
AS
BEGIN
	UPDATE [Nop_CustomerRole_ProductPrice]
	SET
		[CustomerRoleID] = @CustomerRoleID,
		[ProductVariantID] = @ProductVariantID,
		[Price] = @Price
	WHERE
		CustomerRoleProductPriceID = @CustomerRoleProductPriceID
END
