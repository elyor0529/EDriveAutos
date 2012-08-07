-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Delete Products
-- =============================================
CREATE PROCEDURE [dbo].[ED_DeleteEdriveProducts] 
	@EDProductId int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE ED_EdriveProducts SET
	IsActive=0
	WHERE EDProductId=@EDProductId


END

