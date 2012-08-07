

CREATE PROCEDURE [dbo].[Nop_ProductReviewLoadAll]
(
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductReview]
	WHERE IsApproved = 1 or @ShowHidden = 1
	ORDER BY CreatedOn desc
END
