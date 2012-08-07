

CREATE PROCEDURE [dbo].[Nop_ProductTemplateLoadByPrimaryKey]
(
	@ProductTemplateID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductTemplate]
	WHERE
		(ProductTemplateID = @ProductTemplateID)
END
