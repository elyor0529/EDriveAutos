

CREATE PROCEDURE [dbo].[Nop_BlogCommentLoadByPrimaryKey]
(
	@BlogCommentID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_BlogComment]
	WHERE
		BlogCommentID = @BlogCommentID
END
