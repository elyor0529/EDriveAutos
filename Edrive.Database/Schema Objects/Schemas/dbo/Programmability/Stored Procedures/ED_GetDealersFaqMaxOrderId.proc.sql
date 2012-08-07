-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Get max order id of Dealers FAQ for display
-- =============================================

CREATE PROCEDURE [dbo].[ED_GetDealersFaqMaxOrderId]
AS
BEGIN
	SET NOCOUNT ON;

    Select Max(OrderId) From ED_DealersFaq Where IsActive=1
END