

CREATE PROCEDURE [dbo].[Nop_TaxCategoryLoadByPrimaryKey]
(
	@TaxCategoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TaxCategory]
	WHERE
		(TaxCategoryID = @TaxCategoryID)
END
