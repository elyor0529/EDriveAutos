

CREATE PROCEDURE [dbo].[Nop_DiscountLimitationLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_DiscountLimitation]
	ORDER BY DiscountLimitationID
END
