-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Update FAQ Order id
-- =============================================
CREATE PROCEDURE [dbo].[ED_UpdateFaqOrderId]
	@FaqId bigint,
	@OrderId bigint
	
AS
BEGIN
	SET NOCOUNT ON;

    Update ED_Faq set OrderId=@OrderId Where IsActive=1
	And FaqId=@FaqId
END
