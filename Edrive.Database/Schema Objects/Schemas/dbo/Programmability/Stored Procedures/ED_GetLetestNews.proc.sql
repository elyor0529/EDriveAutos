-- =============================================
-- Author:		<Manali>
-- Create date: <28/3/2011>
-- Description:	<Get testimonial for Home page>
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetLetestNews]
	
AS
BEGIN
	SET NOCOUNT ON;
    SELECT top(1) Title,Short,[Full],CreatedOn,NewsID FROM dbo.Nop_News where  Published =1 order by NewsID desc
END