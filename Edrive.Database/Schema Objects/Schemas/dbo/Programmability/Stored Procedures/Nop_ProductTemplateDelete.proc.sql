

CREATE PROCEDURE [dbo].[Nop_ProductTemplateDelete]
(
	@ProductTemplateID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductTemplate]
	WHERE
		[ProductTemplateID] = @ProductTemplateID
END
