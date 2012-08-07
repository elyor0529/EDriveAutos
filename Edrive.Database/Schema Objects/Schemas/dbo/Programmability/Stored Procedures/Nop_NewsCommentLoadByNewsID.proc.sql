

CREATE PROCEDURE [dbo].[Nop_NewsCommentLoadByNewsID]
(
	@NewsID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_NewsComment]
	WHERE
		NewsID=@NewsID
	ORDER BY CreatedOn
END
