

CREATE PROCEDURE [dbo].[Nop_TierPriceDelete]
(
	@TierPriceID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_TierPrice]
	WHERE
		TierPriceID = @TierPriceID
END
