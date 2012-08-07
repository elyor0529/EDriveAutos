

CREATE PROCEDURE [dbo].[Nop_CategoryTemplateDelete]
(
	@CategoryTemplateID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CategoryTemplate]
	WHERE
		[CategoryTemplateID] = @CategoryTemplateID
END
