

CREATE PROCEDURE [dbo].[Nop_ProductPictureInsert]
(
	@ProductPictureID int = NULL output,
	@ProductID int,	
	@PictureID int,	
	@DisplayOrder int,
	@PictureURL varchar(max)
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductPicture]
	(
		ProductID,
		PictureID,
		DisplayOrder,
		PictureURL
	)
	VALUES
	(
		@ProductID,
		@PictureID,
		@DisplayOrder,
		@PictureURL
	)

	set @ProductPictureID=SCOPE_IDENTITY()
END

