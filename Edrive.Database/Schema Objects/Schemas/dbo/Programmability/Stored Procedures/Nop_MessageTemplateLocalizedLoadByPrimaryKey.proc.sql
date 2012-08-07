

CREATE PROCEDURE [dbo].[Nop_MessageTemplateLocalizedLoadByPrimaryKey]
(
	@MessageTemplateLocalizedID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_MessageTemplateLocalized]
	WHERE
		MessageTemplateLocalizedID = @MessageTemplateLocalizedID
END
