

CREATE PROCEDURE [dbo].[Nop_RelatedProductDelete]
(
	@RelatedProductID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_RelatedProduct]
	WHERE
		RelatedProductID = @RelatedProductID
END
