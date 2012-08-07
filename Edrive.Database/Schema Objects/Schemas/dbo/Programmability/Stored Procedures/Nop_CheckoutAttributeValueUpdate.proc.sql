

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueUpdate]
(
	@CheckoutAttributeValueID int,
	@CheckoutAttributeID int,
	@Name nvarchar (100),
	@PriceAdjustment money,
	@WeightAdjustment decimal(18, 4),
	@IsPreSelected bit,
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_CheckoutAttributeValue]
	SET
		CheckoutAttributeID=@CheckoutAttributeID,
		[Name]=@Name,
		PriceAdjustment=@PriceAdjustment,
		WeightAdjustment=@WeightAdjustment,
		IsPreSelected=@IsPreSelected,
		DisplayOrder=@DisplayOrder
	WHERE
		CheckoutAttributeValueID = @CheckoutAttributeValueID
END
