
CREATE PROCEDURE [dbo].[Nop_LanguageInsert]
(
	@LanguageId int = NULL output,
	@Name nvarchar(100),
	@LanguageCulture nvarchar(20),
	@FlagImageFileName nvarchar(50),
	@Published bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_Language]
	(
		[Name],
		[LanguageCulture],
		[FlagImageFileName],
		[Published],
		[DisplayOrder]
	)
	VALUES
	(
		@Name,
		@LanguageCulture,
		@FlagImageFileName,
		@Published,
		@DisplayOrder
	)

	set @LanguageId=SCOPE_IDENTITY()
END
