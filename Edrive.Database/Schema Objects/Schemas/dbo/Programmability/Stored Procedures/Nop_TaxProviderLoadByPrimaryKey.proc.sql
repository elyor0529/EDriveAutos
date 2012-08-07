

CREATE PROCEDURE [dbo].[Nop_TaxProviderLoadByPrimaryKey]
(
	@TaxProviderID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TaxProvider]
	WHERE
		TaxProviderID = @TaxProviderID
END
