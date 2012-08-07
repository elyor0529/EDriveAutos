

CREATE PROCEDURE [dbo].[Nop_TaxCategoryDelete]
(
	@TaxCategoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_TaxCategory]
	WHERE
		TaxCategoryID = @TaxCategoryID
END
