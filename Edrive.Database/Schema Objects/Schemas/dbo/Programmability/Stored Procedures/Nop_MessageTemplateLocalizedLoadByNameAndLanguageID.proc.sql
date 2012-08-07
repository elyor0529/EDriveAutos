

CREATE PROCEDURE [dbo].[Nop_MessageTemplateLocalizedLoadByNameAndLanguageID]
(
	@Name nvarchar(200),
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		mtl.*
	FROM [Nop_MessageTemplateLocalized] mtl
	INNER JOIN [Nop_MessageTemplate] mt
	ON mtl.MessageTemplateID = mt.MessageTemplateID
	WHERE mtl.LanguageID=@LanguageID and mt.[Name] = @Name
END
