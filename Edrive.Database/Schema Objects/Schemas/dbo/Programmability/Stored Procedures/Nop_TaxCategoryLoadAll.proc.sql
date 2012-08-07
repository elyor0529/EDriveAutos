

CREATE PROCEDURE [dbo].[Nop_TaxCategoryLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_TaxCategory]
	order by DisplayOrder
END
