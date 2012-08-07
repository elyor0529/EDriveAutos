

CREATE PROCEDURE [dbo].[Nop_NewsCommentDelete]
(
	@NewsCommentID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_NewsComment]
	WHERE
		[NewsCommentID] = @NewsCommentID
END
