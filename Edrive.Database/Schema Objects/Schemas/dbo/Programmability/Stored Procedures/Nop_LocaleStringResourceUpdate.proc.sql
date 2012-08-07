

CREATE PROCEDURE [dbo].[Nop_LocaleStringResourceUpdate]
(
	@LocaleStringResourceID int,
	@LanguageID int,
	@ResourceName nvarchar(200),
	@ResourceValue nvarchar(MAX)
)
AS
BEGIN
	UPDATE [Nop_LocaleStringResource]
	SET
		LanguageID=@LanguageID,
		ResourceName = @ResourceName,
		ResourceValue = @ResourceValue
	WHERE
		LocaleStringResourceID= @LocaleStringResourceID
END
