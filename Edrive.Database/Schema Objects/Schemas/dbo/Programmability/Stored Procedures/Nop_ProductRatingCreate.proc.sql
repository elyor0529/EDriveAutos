

CREATE PROCEDURE [dbo].[Nop_ProductRatingCreate]
(
	@ProductID int,
	@CustomerID int,
	@Rating int,
	@RatedOn datetime
)
AS
BEGIN

	DELETE FROM Nop_ProductRating WHERE ProductID=@ProductID AND CustomerID=@CustomerID
	
	INSERT
	INTO [Nop_ProductRating]
	(
		ProductID,
		CustomerID,
		Rating,
		RatedOn
	)
	VALUES
	(
		@ProductID,
		@CustomerID,
		@Rating,
		@RatedOn
	)
	
	DECLARE @RatingSum int
	DECLARE @TotalRatingVotes int
	SELECT @RatingSum = SUM(Rating), @TotalRatingVotes = COUNT(ProductRatingID) FROM Nop_ProductRating WHERE ProductID=@ProductID
	UPDATE Nop_Product SET RatingSum=@RatingSum, TotalRatingVotes=@TotalRatingVotes WHERE ProductID=@ProductID
	


END
