-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Update FAQ Order id
-- =============================================
create PROCEDURE [dbo].[ED_UpdateDealersFaqOrderId]
	@FaqId bigint,
	@OrderId bigint
	
AS
BEGIN
	SET NOCOUNT ON;

    Update ED_DealersFaq set OrderId=@OrderId Where IsActive=1
	And FaqId=@FaqId
END