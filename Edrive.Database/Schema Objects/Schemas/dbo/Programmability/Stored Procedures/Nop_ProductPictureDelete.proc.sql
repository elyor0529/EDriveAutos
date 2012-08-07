

CREATE PROCEDURE [dbo].[Nop_ProductPictureDelete]
(
	@ProductPictureID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductPicture]
	WHERE
		ProductPictureID = @ProductPictureID
END
