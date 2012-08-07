-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Insert Expert Document
-- =============================================
--[dbo].[ED_GetTopDealerVehicleDetail] 12,2692
CREATE PROCEDURE [dbo].[ED_GetTopDealerVehicleDetail] --117483,2692
@ProductID  int,
@CustomerID int
	
AS
BEGIN
	SET NOCOUNT OFF
	
	select top(8) p.Name,p.Price_Current,p.ProductId,p.Mileage,p.Drive_Type from nop_product as p
	where p.CustomerID = @CustomerID and p.productID not in(@ProductID) and p.Deleted = 0 and p.Published = 1
	order by p.CreatedOn desc ,p.ProductId desc

END