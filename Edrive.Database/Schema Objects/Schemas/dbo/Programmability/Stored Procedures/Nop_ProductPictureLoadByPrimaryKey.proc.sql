

CREATE PROCEDURE [dbo].[Nop_ProductPictureLoadByPrimaryKey]
(
	@ProductPictureID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductPicture]
	WHERE
		ProductPictureID = @ProductPictureID
END
