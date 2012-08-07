

CREATE PROCEDURE [dbo].[Nop_BlogPostInsert]
(
	@BlogPostID int = NULL output,
	@LanguageID int,
	@BlogPostTitle nvarchar(200),
	@BlogPostBody nvarchar(MAX),
	@BlogPostAllowComments bit,
	@CreatedByID int,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_BlogPost]
	(
		LanguageID,
		BlogPostTitle,
		BlogPostBody,
		BlogPostAllowComments,
		CreatedByID,
		CreatedOn
	)
	VALUES
	(
		@LanguageID,
		@BlogPostTitle,
		@BlogPostBody,
		@BlogPostAllowComments,
		@CreatedByID,
		@CreatedOn
	)

	set @BlogPostID=SCOPE_IDENTITY()
END
