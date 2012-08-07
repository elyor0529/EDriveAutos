

CREATE PROCEDURE [dbo].[Nop_LanguageLoadAll]
	@ShowHidden bit = 0
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Language]
	WHERE (Published = 1 or @ShowHidden = 1)
	order by DisplayOrder
END
