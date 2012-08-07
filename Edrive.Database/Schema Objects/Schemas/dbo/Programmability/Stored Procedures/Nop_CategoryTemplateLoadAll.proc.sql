

CREATE PROCEDURE [dbo].[Nop_CategoryTemplateLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_CategoryTemplate]
	order by DisplayOrder
END
