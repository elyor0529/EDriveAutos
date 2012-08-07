

CREATE PROCEDURE [dbo].[Nop_NewsCommentLoadByPrimaryKey]
(
	@NewsCommentID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_NewsComment]
	WHERE
		NewsCommentID=@NewsCommentID
END
