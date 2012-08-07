-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[ED_GetCodes] --18.165273,18.5271722472496,-67.611895005476,-66.722583
(
	-- Add the parameters for the stored procedure here
	@Lat1 float,
	@Lat2 float,
	@Long1 float,
	@Long2 float
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from ED_Zipcodes where latitude between @Lat1 and @Lat2 and longitude between @Long1 and @Long2
END
