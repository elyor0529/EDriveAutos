

CREATE PROCEDURE [dbo].[Nop_ProductReviewHelpfulnessCreate]
(
	@ProductReviewID int,
	@CustomerID int,
	@WasHelpful bit
)
AS
BEGIN

	DELETE FROM Nop_ProductReviewHelpfulness 
	WHERE ProductReviewID=@ProductReviewID AND 
			CustomerID=@CustomerID
	
	INSERT
	INTO [Nop_ProductReviewHelpfulness]
	(
		ProductReviewID,
		CustomerID,
		WasHelpful
	)
	VALUES
	(
		@ProductReviewID,
		@CustomerID,
		@WasHelpful
	)
	
	DECLARE @HelpfulYesTotal int
	SELECT @HelpfulYesTotal = COUNT(ProductReviewHelpfulnessID) 
	FROM Nop_ProductReviewHelpfulness
	WHERE ProductReviewID=@ProductReviewID and WasHelpful=1

	DECLARE @HelpfulNoTotal int
	SELECT @HelpfulNoTotal = COUNT(ProductReviewHelpfulnessID) 
	FROM Nop_ProductReviewHelpfulness
	WHERE ProductReviewID=@ProductReviewID and WasHelpful=0

	UPDATE Nop_ProductReview
	SET 
		HelpfulYesTotal=@HelpfulYesTotal,
		HelpfulNoTotal=@HelpfulNoTotal 
	WHERE ProductReviewID=@ProductReviewID
	


END
