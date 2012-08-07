

CREATE PROCEDURE [dbo].[Nop_ProductReviewLoadByPrimaryKey]
(
	@ProductReviewID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductReview]
	WHERE
		[ProductReviewID] = @ProductReviewID
END
