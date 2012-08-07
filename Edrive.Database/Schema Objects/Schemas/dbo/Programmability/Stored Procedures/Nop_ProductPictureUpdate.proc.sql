

CREATE PROCEDURE [dbo].[Nop_ProductPictureUpdate]
(
	@ProductPictureID int,
	@ProductID int,	
	@PictureID int,	
	@DisplayOrder int,
	@PictureURL varchar(max)
)
AS
BEGIN

	UPDATE [Nop_ProductPicture]
	SET
		ProductID=@ProductID,
		PictureID=@PictureID,
		DisplayOrder=@DisplayOrder,
		PictureURL=@PictureURL
	WHERE
		ProductPictureID = @ProductPictureID

END

