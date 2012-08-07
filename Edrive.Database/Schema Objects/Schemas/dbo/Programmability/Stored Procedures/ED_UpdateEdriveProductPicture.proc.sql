-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Update picture of product
-- =============================================
CREATE PROCEDURE [dbo].[ED_UpdateEdriveProductPicture]
	@EDProductId int,
	@PictureId int
	
AS
BEGIN
	SET NOCOUNT ON;

    Update ED_EdriveProducts set PictureId=@PictureId Where IsActive=1
	And EDProductId=@EDProductId
END



