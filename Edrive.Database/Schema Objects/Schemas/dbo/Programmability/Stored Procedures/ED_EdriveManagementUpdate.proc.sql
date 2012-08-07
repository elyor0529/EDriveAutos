-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Update Edrive Products
-- =============================================

CREATE PROCEDURE [dbo].[ED_EdriveManagementUpdate]
(
	@ManagementId int,
	@Title nvarchar(250),
	@ShortDesc nvarchar(Max),
	@ImageID int,
	@UpdatedOn datetime,
	@DisplayOrder int,
	@Published bit
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [ED_EManagement]
	SET
		Title=@Title,
		ShortDesc=@ShortDesc,
		ImageID=@ImageID,
		UpdatedOn=@UpdatedOn,
		DisplayOrder=@DisplayOrder,
		Published = @Published
	WHERE
		[ManagementId] = @ManagementId

	SET @Err = @@Error
	RETURN @Err
END