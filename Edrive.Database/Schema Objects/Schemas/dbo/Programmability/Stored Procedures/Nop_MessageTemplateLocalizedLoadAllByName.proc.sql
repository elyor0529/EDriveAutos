

CREATE PROCEDURE [dbo].[Nop_MessageTemplateLocalizedLoadAllByName]
(
	@Name nvarchar(200)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT mtl.*
	FROM [Nop_MessageTemplateLocalized] mtl
	INNER JOIN [Nop_MessageTemplate] mt
	ON mtl.MessageTemplateID = mt.MessageTemplateID
	WHERE mt.[Name] = @Name
	ORDER BY mtl.LanguageID
END
