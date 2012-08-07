

CREATE PROCEDURE [dbo].[Nop_OrderProductVariantDelete]
	@OrderProductVariantID int
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM Nop_OrderProductVariant
	WHERE OrderProductVariantID = @OrderProductVariantID
	
END
