-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <18/2/2011>
-- Description:	Delete Product Data by CustomerId
-- =============================================
CREATE PROCEDURE [dbo].[ED_DeleteProductDataByCustomerID]
(
	@CustomerID int		
)
AS
BEGIN
	DELETE FROM ED_ProductData 
	WHERE CustomerID = @CustomerID
END



