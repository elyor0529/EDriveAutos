

CREATE PROCEDURE [dbo].[Nop_PricelistLoadByPrimaryKey]
(
	@PricelistID int
)
AS
BEGIN
	SELECT *
	FROM
		[Nop_Pricelist]
	WHERE
		[PricelistID] = @PricelistID
END
