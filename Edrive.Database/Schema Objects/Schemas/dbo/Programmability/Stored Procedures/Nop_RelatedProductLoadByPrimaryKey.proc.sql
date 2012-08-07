

CREATE PROCEDURE [dbo].[Nop_RelatedProductLoadByPrimaryKey]
(
	@RelatedProductID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_RelatedProduct]
	WHERE
		RelatedProductID = @RelatedProductID
END
