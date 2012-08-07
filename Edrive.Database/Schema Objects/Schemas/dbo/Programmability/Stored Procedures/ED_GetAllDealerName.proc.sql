-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetAllDealerName] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	Declare @iDays as int
	Select @iDays = [Value] FROM Nop_Setting where Name='CountDown.Days'
	
	select distinct ca.Value as Name,c.CustomerID from Nop_Customer as c
	left outer join
	Nop_CustomerAttribute as ca
	on c.CustomerID=ca.CustomerId and ca.[Key]='Company'

	left outer join 
	Nop_Product as p 
	on p.CustomerID = c.CustomerID
	where CustomerType =1 and c.active=1 and c.Deleted=0 and 
	DATEDIFF(s,getdate(),DATEADD(d,@iDays,UpdatedOn)) > 0 And p.Deleted=0 And p.Published=1 And p.QualifyPrice is not null order by ca.Value


END