

CREATE PROCEDURE [dbo].[Nop_CampaignDelete]
(
	@CampaignID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Campaign]
	WHERE
		CampaignID = @CampaignID
END
