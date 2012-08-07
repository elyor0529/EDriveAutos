

CREATE PROCEDURE [dbo].[Nop_BlogCommentUpdate]
(
	@BlogCommentID int,
	@BlogPostID int,
	@CustomerID int,
	@IPAddress nvarchar(100),
	@CommentText nvarchar(MAX),
	@CreatedOn datetime
)
AS
BEGIN

	UPDATE [Nop_BlogComment]
	SET
		BlogPostID=@BlogPostID,
		CustomerID=@CustomerID,
		IPAddress=@IPAddress,
		CommentText=@CommentText,
		CreatedOn=@CreatedOn
	WHERE
		BlogCommentID = @BlogCommentID
END
