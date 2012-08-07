

CREATE PROCEDURE [dbo].[Nop_TaxProviderLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_TaxProvider]
	order by DisplayOrder
END
