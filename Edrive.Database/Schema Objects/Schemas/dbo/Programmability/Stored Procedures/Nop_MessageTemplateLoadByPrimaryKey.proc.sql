

CREATE PROCEDURE [dbo].[Nop_MessageTemplateLoadByPrimaryKey]
(
	@MessageTemplateID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_MessageTemplate]
	WHERE
		MessageTemplateID = @MessageTemplateID
END
