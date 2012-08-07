

CREATE PROCEDURE [dbo].[Nop_DiscountInsert]
(
	@DiscountID int = NULL output,
	@DiscountTypeID int,
	@DiscountRequirementID int,
	@DiscountLimitationID int,
	@Name nvarchar(100),
	@UsePercentage bit, 
	@DiscountPercentage decimal (18, 4),
	@DiscountAmount decimal (18, 4),
	@StartDate datetime,
	@EndDate datetime,
	@RequiresCouponCode bit,
	@CouponCode nvarchar(100),
	@Deleted bit
)
AS
BEGIN
	INSERT
	INTO [Nop_Discount]
	(
		[DiscountTypeID],
		[DiscountRequirementID],
		[DiscountLimitationID],
		[Name],
		[UsePercentage],
		[DiscountPercentage],
		[DiscountAmount],
		[StartDate],
		[EndDate],
		[RequiresCouponCode],
		[CouponCode],
		[Deleted]
	)
	VALUES
	(
		@DiscountTypeID,
		@DiscountRequirementID,
		@DiscountLimitationID,
		@Name,
		@UsePercentage,
		@DiscountPercentage,
		@DiscountAmount,
		@StartDate,
		@EndDate,
		@RequiresCouponCode,
		@CouponCode,
		@Deleted
	)

	set @DiscountID=SCOPE_IDENTITY()
END
