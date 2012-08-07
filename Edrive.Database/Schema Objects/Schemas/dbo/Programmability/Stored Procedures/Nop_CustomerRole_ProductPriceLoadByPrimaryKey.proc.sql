

CREATE PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceLoadByPrimaryKey]
(
	@CustomerRoleProductPriceID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CustomerRole_ProductPrice]
	WHERE
		[CustomerRoleProductPriceID] = @CustomerRoleProductPriceID
END
