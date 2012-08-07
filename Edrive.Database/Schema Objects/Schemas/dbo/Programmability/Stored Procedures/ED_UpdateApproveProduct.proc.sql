-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Get Dealers FAQ
-- =============================================
CREATE PROCEDURE [dbo].[ED_UpdateApproveProduct]
	
AS
BEGIN
	SET NOCOUNT ON;
    Update Nop_Product
    set QualifyPrice = 0.0
    where Deleted = 0
END