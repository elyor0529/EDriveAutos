

CREATE PROCEDURE [dbo].[Nop_LocaleStringResourceInsert]
(
	@LocaleStringResourceID int = NULL output,
	@LanguageID int,
	@ResourceName nvarchar(200),
	@ResourceValue nvarchar(MAX)
)
AS
BEGIN
	INSERT
	INTO [Nop_LocaleStringResource]
	(
		[LanguageID],
		[ResourceName],
		[ResourceValue]
	)
	VALUES
	(
		@LanguageID,
		@ResourceName,
		@ResourceValue
	)

	set @LocaleStringResourceID=SCOPE_IDENTITY()
END
