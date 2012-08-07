

CREATE PROCEDURE [dbo].[Nop_ProductAttributeDelete]
(
	@ProductAttributeID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductAttribute]
	WHERE
		ProductAttributeID = @ProductAttributeID
END
