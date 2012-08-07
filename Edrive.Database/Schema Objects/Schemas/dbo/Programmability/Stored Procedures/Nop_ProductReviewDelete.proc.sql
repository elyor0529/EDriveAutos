

CREATE PROCEDURE [dbo].[Nop_ProductReviewDelete]
(
	@ProductReviewID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductReview]
	WHERE
		ProductReviewID = @ProductReviewID
END
