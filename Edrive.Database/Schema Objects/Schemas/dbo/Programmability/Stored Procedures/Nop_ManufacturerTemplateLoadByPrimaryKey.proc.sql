

CREATE PROCEDURE [dbo].[Nop_ManufacturerTemplateLoadByPrimaryKey]
(
	@ManufacturerTemplateID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ManufacturerTemplate]
	WHERE
		(ManufacturerTemplateID = @ManufacturerTemplateID)
END
