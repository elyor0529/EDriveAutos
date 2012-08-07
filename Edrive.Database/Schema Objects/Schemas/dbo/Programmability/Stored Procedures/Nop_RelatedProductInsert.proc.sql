

CREATE PROCEDURE [dbo].[Nop_RelatedProductInsert]
(
	@RelatedProductID int = NULL output,
	@ProductID1 int,	
	@ProductID2 int,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_RelatedProduct]
	(
		ProductID1,
		ProductID2,
		DisplayOrder
	)
	VALUES
	(
		@ProductID1,
		@ProductID2,
		@DisplayOrder
	)

	set @RelatedProductID=SCOPE_IDENTITY()
END
