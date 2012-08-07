-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <22/2/2011>
-- Description:	Get updated products
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetUpdatedProducts]
		
AS
BEGIN
	SET NOCOUNT ON;

    Select * From Nop_Product
	Where IsNew=0
END


