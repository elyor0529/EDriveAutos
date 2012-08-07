-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <16/2/2011>
-- Description:	Get uploaded products's latest date
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetProductDataLatestDates]
	@CustomerID int
AS
BEGIN
	
	Select Top 1 CreatedOn as Date
	FROM 
	ED_ProductData
	WHERE CustomerID = @CustomerID
	
END





