

CREATE PROCEDURE [dbo].[Nop_CampaignInsert]
(
	@CampaignID int = NULL output,
	@Name nvarchar(200),
	@Subject nvarchar(200),
	@Body nvarchar(MAX),
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Campaign]
	(
		[Name],
		[Subject],
		Body,
		CreatedOn
	)
	VALUES
	(
		@Name,
		@Subject,
		@Body,
		@CreatedOn
	)

	set @CampaignID=SCOPE_IDENTITY()
END
