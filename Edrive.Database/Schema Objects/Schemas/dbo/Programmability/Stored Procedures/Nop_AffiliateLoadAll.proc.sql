

CREATE PROCEDURE [dbo].[Nop_AffiliateLoadAll]

AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Affiliate]
	WHERE Deleted=0
	ORDER BY LastName
END
