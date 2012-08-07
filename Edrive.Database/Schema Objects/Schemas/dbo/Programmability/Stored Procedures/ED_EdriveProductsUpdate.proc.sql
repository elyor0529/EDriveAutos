-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Update Edrive Products
-- =============================================

CREATE PROCEDURE [dbo].[ED_EdriveProductsUpdate]
(
	@EDProductId int,
	@Title nvarchar(100),
	@ShortDescription nvarchar(255),
	@FullDescription nvarchar(MAX),
	@PictureId int,
	@UpdatedOn datetime,
	@DisplayOrder int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [ED_EdriveProducts]
	SET
		Title=@Title,
		ShortDescription=@ShortDescription,
		FullDescription=@FullDescription,
		PictureId=@PictureId,
		UpdatedOn=@UpdatedOn,
		DisplayOrder=@DisplayOrder
	WHERE
		[EDProductId] = @EDProductId

	SET @Err = @@Error
	RETURN @Err
END