-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetAllMappDealerName] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	
	select ca.Value as Name,c.CustomerID from Nop_Customer as c
	left outer join
	Nop_CustomerAttribute as ca
	on c.CustomerID=ca.CustomerId and ca.[Key]='Company'

		
	where CustomerType =1 and c.active=1 and c.Deleted=0 and (c.DealerCityId is NULL or c.DealerCityId=0)
	
	order by ca.Value


END