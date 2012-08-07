

CREATE PROCEDURE [dbo].[Nop_ProductReviewInsert]
(
	@ProductReviewID int = NULL output,
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
	INSERT
	INTO [Nop_ProductReview]
	(
		ProductID,
		CustomerID,
		IPAddress,
		Title,
		ReviewText,
		Rating,
		HelpfulYesTotal,
		HelpfulNoTotal,
		IsApproved,
		CreatedOn
	)
	VALUES
	(
		@ProductID,
		@CustomerID,
		@IPAddress,
		@Title,
		@ReviewText,
		@Rating,
		@HelpfulYesTotal,
		@HelpfulNoTotal,
		@IsApproved,
		@CreatedOn
	)

	set @ProductReviewID=SCOPE_IDENTITY()
END
