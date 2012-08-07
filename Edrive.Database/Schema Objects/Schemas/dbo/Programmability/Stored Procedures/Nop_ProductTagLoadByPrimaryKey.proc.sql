

CREATE PROCEDURE [dbo].[Nop_ProductTagLoadByPrimaryKey]
(
	@ProductTagID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductTag]
	WHERE
		ProductTagID = @ProductTagID
END
