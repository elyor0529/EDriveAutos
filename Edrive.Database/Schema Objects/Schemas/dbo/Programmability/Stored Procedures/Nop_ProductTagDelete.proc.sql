

CREATE PROCEDURE [dbo].[Nop_ProductTagDelete]
(
	@ProductTagID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductTag]
	WHERE
		ProductTagID = @ProductTagID
END
