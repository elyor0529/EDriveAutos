

CREATE PROCEDURE [dbo].[Nop_MessageTemplateLocalizedUpdate]
(
	@MessageTemplateLocalizedID int,
	@MessageTemplateID int,
	@LanguageID int,
	@BCCEmailAddresses nvarchar(200),	
	@Subject nvarchar(200),
	@Body nvarchar(MAX),
	@IsActive bit
)
AS
BEGIN

	UPDATE [Nop_MessageTemplateLocalized]
	SET
		MessageTemplateID=@MessageTemplateID,
		LanguageID=@LanguageID,
		BCCEmailAddresses=@BCCEmailAddresses,
		[Subject]=@Subject,
		Body=@Body,
		IsActive=@IsActive
	WHERE
		MessageTemplateLocalizedID = @MessageTemplateLocalizedID

END
