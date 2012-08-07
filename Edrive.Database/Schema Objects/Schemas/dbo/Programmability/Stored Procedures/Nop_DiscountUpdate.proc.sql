

CREATE PROCEDURE [dbo].[Nop_DiscountUpdate]
(
	@DiscountID int,
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
	UPDATE [Nop_Discount]
	SET
		[DiscountTypeID]=@DiscountTypeID,
		[DiscountRequirementID]=@DiscountRequirementID,
		[DiscountLimitationID]=@DiscountLimitationID,
		[Name]=@Name,
		[UsePercentage]=@UsePercentage,
		[DiscountPercentage]=@DiscountPercentage,
		[DiscountAmount]=@DiscountAmount,
		[StartDate]=@StartDate,
		[EndDate]=@EndDate,
		[RequiresCouponCode]=@RequiresCouponCode,
		[CouponCode]=@CouponCode,
		[Deleted]=@Deleted
	WHERE
		[DiscountID] = @DiscountID
END
