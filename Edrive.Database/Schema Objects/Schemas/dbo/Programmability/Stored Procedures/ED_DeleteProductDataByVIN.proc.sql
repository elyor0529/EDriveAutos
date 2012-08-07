-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <14/2/2011>
-- Description:	Delete Product data by VIN
-- =============================================
CREATE PROCEDURE [dbo].[ED_DeleteProductDataByVIN]
(
	@VIN varchar(100),
	@CustomerID int		
)
AS
BEGIN
	Delete 
	From 
	ED_ProductData
	Where VIN = @VIN And CustomerID = @CustomerID
END



