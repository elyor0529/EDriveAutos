

CREATE PROCEDURE [dbo].[Nop_SearchLogLoadAll]

AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_SearchLog]
	ORDER BY CreatedOn desc
END
