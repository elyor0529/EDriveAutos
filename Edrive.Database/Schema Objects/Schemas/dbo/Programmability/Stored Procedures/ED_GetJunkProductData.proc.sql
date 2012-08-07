-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <18/2/2011>
-- Description:	Get junk product data
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetJunkProductData] --37
	@CustomerID int,
	@FileName varchar(max)
AS
BEGIN
	Select VIN,CustomerID,VehicleName,Price_Current,CreatedOn,[FileName]
	From
	ED_JunkProductData
	Where 1=1
	And CustomerID = @CustomerID And [FileName]=@FileName
	Order BY CreatedOn desc
END










