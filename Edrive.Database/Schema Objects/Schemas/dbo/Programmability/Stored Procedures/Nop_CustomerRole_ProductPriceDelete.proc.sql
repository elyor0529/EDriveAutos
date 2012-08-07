

CREATE PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceDelete]
(
	@CustomerRoleProductPriceID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CustomerRole_ProductPrice]
	WHERE
		[CustomerRoleProductPriceID] = @CustomerRoleProductPriceID
END
