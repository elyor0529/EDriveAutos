-- =============================================
-- Author:		<Manali>
-- Create date: <28/3/2011>
-- Description:	<Get testimonial for Home page>
-- =============================================
create PROCEDURE [dbo].[ED_GetLetestNewsById]
@NewsID	int
AS
BEGIN
	SET NOCOUNT ON;
    SELECT * FROM dbo.Nop_News where Published =1 and
    NewsID=@NewsID
END


