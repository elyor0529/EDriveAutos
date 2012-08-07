

CREATE PROCEDURE [dbo].[Nop_BlogCommentLoadByBlogPostID]
(
	@BlogPostID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_BlogComment]
	WHERE
		BlogPostID=@BlogPostID
	ORDER BY CreatedOn
END
