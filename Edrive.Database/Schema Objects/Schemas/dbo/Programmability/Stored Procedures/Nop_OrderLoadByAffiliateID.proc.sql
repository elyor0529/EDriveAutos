

CREATE PROCEDURE [dbo].[Nop_OrderLoadByAffiliateID]
(
	@AffiliateID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Order]
	WHERE
		AffiliateID=@AffiliateID and Deleted=0
	ORDER BY CreatedOn desc
END
