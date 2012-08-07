

CREATE PROCEDURE [dbo].[Nop_TierPriceUpdate]
(
	@TierPriceID int,
	@ProductVariantID int,
	@Quantity int,
	@Price money
)
AS
BEGIN
	UPDATE [Nop_TierPrice]
	SET
	ProductVariantID=@ProductVariantID,
	[Quantity]=@Quantity,
	[Price]=@Price
	WHERE
		TierPriceID = @TierPriceID
END
