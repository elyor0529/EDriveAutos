-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Get max order id of FAQ for display
-- =============================================

CREATE PROCEDURE [dbo].[ED_GetFaqMaxOrderId]
AS
BEGIN
	SET NOCOUNT ON;

    Select Max(OrderId) From ED_Faq Where IsActive=1
END

