

CREATE PROCEDURE [dbo].[Nop_ProductTagUpdate]
(
	@ProductTagID int,
	@Name nvarchar(100),
	@ProductCount int
)
AS
BEGIN

	UPDATE [Nop_ProductTag]
	SET
		[Name]=@Name,
		[ProductCount]=@ProductCount
	WHERE
		ProductTagID = @ProductTagID

END
