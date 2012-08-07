

CREATE PROCEDURE [dbo].[Nop_MessageTemplateLocalizedDelete]
(
	@MessageTemplateLocalizedID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_MessageTemplateLocalized]
	WHERE
		MessageTemplateLocalizedID = @MessageTemplateLocalizedID
END
