-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Insert Edrive Products
-- =============================================

CREATE PROCEDURE [dbo].[ED_EdriveProductInsert]
(
	@EDProductId int = NULL output,
	@Title nvarchar(100),
	@ShortDescription nvarchar(255),
	@FullDescription nvarchar(MAX),
	@PictureId int,
	@IsActive bit,
	@CreatedOn datetime,
	@UpdatedOn datetime = NULL,
	@DisplayOrder int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	INSERT
	INTO [ED_EdriveProducts]
	(
		Title,
		ShortDescription,
		FullDescription,
		PictureId,
		IsActive,
		CreatedOn,
		UpdatedOn,
		DisplayOrder
	)
	VALUES
	(
		@Title,
		@ShortDescription,
		@FullDescription,
		@PictureId,
		@IsActive,
		@CreatedOn,
		@UpdatedOn,
		@DisplayOrder
	)

	SET @Err = @@Error

	SELECT @EDProductId = SCOPE_IDENTITY()

	RETURN @Err
END