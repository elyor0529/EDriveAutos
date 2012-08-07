

CREATE PROCEDURE [dbo].[Nop_ManufacturerTemplateDelete]
(
	@ManufacturerTemplateID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ManufacturerTemplate]
	WHERE
		[ManufacturerTemplateID] = @ManufacturerTemplateID
END
