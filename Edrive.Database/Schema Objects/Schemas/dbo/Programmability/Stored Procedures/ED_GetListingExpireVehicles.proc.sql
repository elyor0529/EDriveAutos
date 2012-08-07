-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[ED_GetListingExpireVehicles] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	Declare @iDays as int
	Select @iDays = [Value] FROM Nop_Setting where Name='CountDown.Days'

	select top(8) * from Nop_Product as p where DATEDIFF(s,getdate(),DATEADD(d,@iDays,UpdatedOn)) > 0 And p.Deleted=0 And p.Published=1 And p.QualifyPrice is not null 
	Order By UpdatedOn Asc
END