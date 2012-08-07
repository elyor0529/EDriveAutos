

CREATE PROCEDURE [dbo].[Nop_BlogPostLoadByPrimaryKey]
(
	@BlogPostID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_BlogPost]
	WHERE
		BlogPostID = @BlogPostID
END
