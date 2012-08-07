

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueInsert]
(
	@CheckoutAttributeValueID int = NULL output,
	@CheckoutAttributeID int,
	@Name nvarchar (100),
	@PriceAdjustment money,
	@WeightAdjustment decimal(18, 4),
	@IsPreSelected bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_CheckoutAttributeValue]
	(
		[CheckoutAttributeID],
		[Name],
		[PriceAdjustment],
		[WeightAdjustment],
		[IsPreSelected],
		[DisplayOrder]
	)
	VALUES
	(
		@CheckoutAttributeID,
		@Name,
		@PriceAdjustment,
		@WeightAdjustment,
		@IsPreSelected,
		@DisplayOrder
	)

	set @CheckoutAttributeValueID=SCOPE_IDENTITY()
END
