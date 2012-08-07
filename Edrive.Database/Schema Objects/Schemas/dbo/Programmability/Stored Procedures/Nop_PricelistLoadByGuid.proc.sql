

CREATE PROCEDURE [dbo].[Nop_PricelistLoadByGuid]
(
	@PricelistGuid nvarchar(40)
)
AS
BEGIN
	SELECT *
	FROM
		[Nop_Pricelist]
	WHERE
		[PricelistGuid] = @PricelistGuid
END
