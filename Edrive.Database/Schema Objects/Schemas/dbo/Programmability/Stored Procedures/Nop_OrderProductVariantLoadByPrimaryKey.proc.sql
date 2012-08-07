

CREATE PROCEDURE [dbo].[Nop_OrderProductVariantLoadByPrimaryKey]
(
	@OrderProductVariantID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_OrderProductVariant]
	WHERE
		OrderProductVariantID = @OrderProductVariantID
END
