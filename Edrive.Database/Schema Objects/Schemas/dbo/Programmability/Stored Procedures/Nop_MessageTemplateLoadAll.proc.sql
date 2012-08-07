

CREATE PROCEDURE [dbo].[Nop_MessageTemplateLoadAll]

AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_MessageTemplate]
	ORDER BY [Name]
END
