

CREATE PROCEDURE [dbo].[Nop_RelatedProductUpdate]
(
	@RelatedProductID int,
	@ProductID1 int,
	@ProductID2 int,	
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_RelatedProduct]
	SET
		ProductID1=@ProductID1,
		ProductID2=@ProductID2,
		DisplayOrder=@DisplayOrder
	WHERE
		RelatedProductID = @RelatedProductID

END
