

CREATE PROCEDURE [dbo].[Nop_AffiliateLoadByPrimaryKey]
(
	@AffiliateID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Affiliate]
	WHERE
		AffiliateID = @AffiliateID
END
