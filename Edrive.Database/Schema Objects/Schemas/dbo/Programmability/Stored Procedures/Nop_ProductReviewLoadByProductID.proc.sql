

CREATE PROCEDURE [dbo].[Nop_ProductReviewLoadByProductID]
(
	@ProductID int,
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductReview]
	WHERE
		ProductID=@ProductID
		AND (IsApproved = 1 or @ShowHidden = 1)
	ORDER BY CreatedOn desc
END
