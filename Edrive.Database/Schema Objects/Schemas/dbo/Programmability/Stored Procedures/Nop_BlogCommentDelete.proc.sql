

CREATE PROCEDURE [dbo].[Nop_BlogCommentDelete]
(
	@BlogCommentID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_BlogComment]
	WHERE
		BlogCommentID = @BlogCommentID
END
