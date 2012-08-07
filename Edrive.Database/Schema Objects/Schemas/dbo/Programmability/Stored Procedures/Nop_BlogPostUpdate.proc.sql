

CREATE PROCEDURE [dbo].[Nop_BlogPostUpdate]
(
	@BlogPostID int,
	@LanguageID int,
	@BlogPostTitle nvarchar(200),
	@BlogPostBody nvarchar(MAX),
	@BlogPostAllowComments bit,
	@CreatedByID int,
	@CreatedOn datetime
)
AS
BEGIN

	UPDATE [Nop_BlogPost]
	SET
		LanguageID=@LanguageID,
		BlogPostTitle=@BlogPostTitle,
		BlogPostBody=@BlogPostBody,
		BlogPostAllowComments=@BlogPostAllowComments,
		CreatedByID=@CreatedByID
	WHERE
		BlogPostID = @BlogPostID

END
