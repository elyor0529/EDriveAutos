-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <22/2/2011>
-- Description:	Delete Product Images that are added by auto processing
-- =============================================
CREATE PROCEDURE [dbo].[ED_DeleteProductPicture]
	@ProductID int
AS
BEGIN
	SET NOCOUNT ON;

    Delete From Nop_ProductPicture
	Where ProductID=@ProductID And PictureId=0
END


