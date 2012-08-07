

CREATE PROCEDURE [dbo].[Nop_TierPriceInsert]
(
	@TierPriceID int = NULL output,
	@ProductVariantID int,
	@Quantity int,
	@Price money
)
AS
BEGIN
	INSERT
	INTO [Nop_TierPrice]
	(
		[ProductVariantID],
		[Quantity],
		[Price]
	)
	VALUES
	(
		@ProductVariantID,
		@Quantity,
		@Price
	)

	set @TierPriceID=SCOPE_IDENTITY()
END
