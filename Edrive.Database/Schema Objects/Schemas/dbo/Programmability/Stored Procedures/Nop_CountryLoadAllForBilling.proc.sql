

CREATE PROCEDURE [dbo].[Nop_CountryLoadAllForBilling]
	@ShowHidden bit = 0
AS
BEGIN
	SELECT *
	FROM [Nop_Country]
	WHERE (Published = 1 or @ShowHidden = 1) and AllowsBilling=1
	ORDER BY DisplayOrder, [Name]
END
