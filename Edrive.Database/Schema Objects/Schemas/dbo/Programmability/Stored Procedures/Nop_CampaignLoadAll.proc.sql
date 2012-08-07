

CREATE PROCEDURE [dbo].[Nop_CampaignLoadAll]

AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Campaign]
	ORDER BY [CreatedOn] desc
END
