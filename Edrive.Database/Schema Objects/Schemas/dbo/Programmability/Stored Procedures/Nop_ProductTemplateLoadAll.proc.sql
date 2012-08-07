

CREATE PROCEDURE [dbo].[Nop_ProductTemplateLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ProductTemplate]
	order by DisplayOrder
END
