

CREATE PROCEDURE [dbo].[Nop_ManufacturerTemplateLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ManufacturerTemplate]
	order by DisplayOrder
END
