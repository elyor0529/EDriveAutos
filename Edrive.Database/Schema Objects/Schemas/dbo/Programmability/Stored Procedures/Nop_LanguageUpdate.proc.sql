

CREATE PROCEDURE [dbo].[Nop_LanguageUpdate]
(
	@LanguageId int,
	@Name nvarchar(100),
	@LanguageCulture nvarchar(20),
	@FlagImageFileName nvarchar(50),
	@Published bit,
	@DisplayOrder int
)
AS
BEGIN
	UPDATE [Nop_Language]
	SET
		[Name] = @Name,
		[LanguageCulture] = @LanguageCulture,
		[FlagImageFileName] = @FlagImageFileName,
		[Published] = @Published,
		[DisplayOrder] = @DisplayOrder
	WHERE
		[LanguageId] = @LanguageId
END
