

CREATE PROCEDURE [dbo].[Nop_ProductReviewUpdate]
(
	@ProductReviewID int,
	@ProductID int,
	@CustomerID int,
	@IPAddress nvarchar(100),
	@Title nvarchar(1000),
	@ReviewText nvarchar(max),
	@Rating int,
	@HelpfulYesTotal int,
	@HelpfulNoTotal int,
	@IsApproved bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ProductReview]
	SET
		ProductID=@ProductID,
		CustomerID=@CustomerID,
		IPAddress=@IPAddress,
		Title=@Title,
		ReviewText=@ReviewText,
		Rating=@Rating,
		HelpfulYesTotal=@HelpfulYesTotal,
		HelpfulNoTotal=@HelpfulNoTotal,
		IsApproved=@IsApproved,
		CreatedOn=@CreatedOn
	WHERE
		ProductReviewID = @ProductReviewID
END
