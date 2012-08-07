

CREATE PROCEDURE [dbo].[Nop_OrderProductVariantLoadByGUID]
(
	@OrderProductVariantGUID uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_OrderProductVariant]
	WHERE
		OrderProductVariantGUID = @OrderProductVariantGUID
END
