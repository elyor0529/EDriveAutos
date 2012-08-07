-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetAllModelByMake] --'Acura'
	-- Add the parameters for the stored procedure here
	@Make varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select distinct model,MakeAttribute,make from Nop_Product
	where Make =@Make

END