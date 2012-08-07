

CREATE PROCEDURE [dbo].[Nop_BlogCommentInsert]
(
	@BlogCommentID int = NULL output,
	@BlogPostID int,
	@CustomerID int,
	@IPAddress nvarchar(100),
	@CommentText nvarchar(MAX),
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_BlogComment]
	(
		BlogPostID,
		CustomerID,
		IPAddress,
		CommentText,
		CreatedOn
	)
	VALUES
	(
		@BlogPostID,
		@CustomerID,
		@IPAddress,
		@CommentText,
		@CreatedOn
	)

	set @BlogCommentID=SCOPE_IDENTITY()
END
