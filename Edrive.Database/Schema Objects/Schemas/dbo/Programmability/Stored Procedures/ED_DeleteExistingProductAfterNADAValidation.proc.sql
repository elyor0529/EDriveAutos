-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ED_DeleteExistingProductAfterNADAValidation] 
	-- Add the parameters for the stored procedure here
	@VIN varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update Nop_Product Set Deleted=1 Where VIN=@VIN
END