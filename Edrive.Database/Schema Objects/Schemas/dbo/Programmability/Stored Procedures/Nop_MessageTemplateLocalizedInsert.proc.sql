

CREATE PROCEDURE [dbo].[Nop_MessageTemplateLocalizedInsert]
(
	@MessageTemplateLocalizedID int = NULL output,
	@MessageTemplateID int,
	@LanguageID int,
	@BCCEmailAddresses nvarchar(200),
	@Subject nvarchar(200),
	@Body nvarchar(MAX),
	@IsActive bit
)
AS
BEGIN
	INSERT
	INTO [Nop_MessageTemplateLocalized]
	(
		MessageTemplateID,
		LanguageID,
		BCCEmailAddresses,
		[Subject],
		Body,
		IsActive
	)
	VALUES
	(
		@MessageTemplateID,
		@LanguageID,
		@BCCEmailAddresses,
		@Subject,
		@Body,
		@IsActive
	)

	set @MessageTemplateLocalizedID=SCOPE_IDENTITY()
END
