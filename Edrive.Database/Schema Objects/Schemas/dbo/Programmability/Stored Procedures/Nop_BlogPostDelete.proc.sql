

CREATE PROCEDURE [dbo].[Nop_BlogPostDelete]
(
	@BlogPostID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_BlogPost]
	WHERE
		BlogPostID = @BlogPostID
END
