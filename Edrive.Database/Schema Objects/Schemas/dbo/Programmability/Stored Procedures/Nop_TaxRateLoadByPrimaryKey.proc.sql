

CREATE PROCEDURE [dbo].[Nop_TaxRateLoadByPrimaryKey]
(
	@TaxRateID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TaxRate]
	WHERE
		TaxRateID = @TaxRateID
END
