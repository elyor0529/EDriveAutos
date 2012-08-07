

CREATE PROCEDURE [dbo].[Nop_PricelistDelete]
(
	@PricelistID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Pricelist]
	WHERE
		PricelistID = @PricelistID
END
