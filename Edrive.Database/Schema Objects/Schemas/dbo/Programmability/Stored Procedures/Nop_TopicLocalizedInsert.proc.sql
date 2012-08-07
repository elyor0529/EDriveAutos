

CREATE PROCEDURE [dbo].[Nop_TopicLocalizedInsert]
(
	@TopicLocalizedID int = NULL output,
	@TopicID int,
	@LanguageID int,
	@Title nvarchar(200),
	@Body nvarchar(MAX),
	@CreatedOn datetime,
	@UpdatedOn datetime,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400)
)
AS
BEGIN
	INSERT
	INTO [Nop_TopicLocalized]
	(
		TopicID,
		LanguageID,
		[Title],
		Body,
		CreatedOn,
		UpdatedOn,
		MetaKeywords,
		MetaDescription,
		MetaTitle
	)
	VALUES
	(
		@TopicID,
		@LanguageID,
		@Title,
		@Body,
		@CreatedOn,
		@UpdatedOn,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle
	)

	set @TopicLocalizedID=SCOPE_IDENTITY()
END
