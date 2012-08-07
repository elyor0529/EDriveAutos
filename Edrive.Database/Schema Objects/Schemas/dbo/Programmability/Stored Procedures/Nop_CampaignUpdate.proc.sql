

CREATE PROCEDURE [dbo].[Nop_CampaignUpdate]
(
	@CampaignID int,
	@Name nvarchar(200),	
	@Subject nvarchar(200),
	@Body nvarchar(MAX),
	@CreatedOn datetime
)
AS
BEGIN

	UPDATE [Nop_Campaign]
	SET
		[Name]=@Name,
		[Subject]=@Subject,
		Body=@Body,
		CreatedOn=@CreatedOn
	WHERE
		CampaignID = @CampaignID

END
