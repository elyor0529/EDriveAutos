-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Delete FAQ
-- =============================================
CREATE PROCEDURE [dbo].[ED_UpdateProductVariant] --'134'
@CustomerId int
AS
BEGIN
	
	SET NOCOUNT ON;
	UPDATE Nop_ProductVariant SET
	OrderMaximumQuantity = 300

END