

CREATE PROCEDURE [dbo].[Nop_CountryLoadAll]
	@ShowHidden bit = 0
AS
BEGIN
	SELECT *
	FROM [Nop_Country]
	WHERE (Published = 1 or @ShowHidden = 1)
	ORDER BY DisplayOrder, [Name]
END
