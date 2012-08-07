-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <16/2/2011>
-- Description:	Get Uploaded products data
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetUploadedProductData] --null,7
	@VIN varchar(max) = null,
	@CustomerID int
AS
BEGIN
	Select VIN,VehicleName,Pics,Price_Current	
	From
	ED_ProductData as EP
	Where 1=1
	and (@VIN is NULL or EP.VIN = @VIN)
	and EP.CustomerID = @CustomerID
	Order BY VIN
END







