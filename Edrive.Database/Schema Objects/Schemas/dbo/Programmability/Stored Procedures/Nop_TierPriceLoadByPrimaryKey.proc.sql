

CREATE PROCEDURE [dbo].[Nop_TierPriceLoadByPrimaryKey]
(
	@TierPriceID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TierPrice]
	WHERE
		TierPriceID = @TierPriceID
END
