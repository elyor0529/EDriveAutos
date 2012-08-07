

CREATE PROCEDURE [dbo].[Nop_PictureInsert]
(
	@PictureID int = NULL output,
	@PictureBinary varbinary(MAX),	
	@Extension nvarchar(20),
	@IsNew	bit
)
AS
BEGIN
	INSERT
	INTO [Nop_Picture]
	(
		PictureBinary,
		Extension,
		IsNew
	)
	VALUES
	(
		@PictureBinary,
		@Extension,
		@IsNew
	)

	set @PictureID=SCOPE_IDENTITY()
END
