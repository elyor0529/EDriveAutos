

CREATE PROCEDURE [dbo].[Nop_CategoryTemplateLoadByPrimaryKey]
(
	@CategoryTemplateID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CategoryTemplate]
	WHERE
		(CategoryTemplateID = @CategoryTemplateID)
END
