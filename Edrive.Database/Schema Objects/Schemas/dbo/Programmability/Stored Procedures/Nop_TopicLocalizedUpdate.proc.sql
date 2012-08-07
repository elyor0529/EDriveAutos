

CREATE PROCEDURE [dbo].[Nop_TopicLocalizedUpdate]
(
	@TopicLocalizedID int,
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

	UPDATE [Nop_TopicLocalized]
	SET
		TopicID=@TopicID,
		LanguageID=@LanguageID,
		[Title]=@Title,
		Body=@Body,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn,
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle 
	WHERE
		TopicLocalizedID = @TopicLocalizedID
END
