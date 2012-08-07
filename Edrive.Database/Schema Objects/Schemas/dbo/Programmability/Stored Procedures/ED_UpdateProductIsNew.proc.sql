-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <22/2/2011>
-- Description:	Update IsNew flag in product table
-- =============================================
CREATE PROCEDURE [dbo].[ED_UpdateProductIsNew]
		
AS
BEGIN
	SET NOCOUNT ON;

    Update Nop_Product
	Set IsNew=1 Where Deleted=0
END


