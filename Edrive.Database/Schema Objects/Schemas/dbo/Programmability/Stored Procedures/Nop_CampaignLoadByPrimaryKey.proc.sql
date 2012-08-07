

CREATE PROCEDURE [dbo].[Nop_CampaignLoadByPrimaryKey]
(
	@CampaignID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Campaign]
	WHERE
		CampaignID = @CampaignID
END
