

CREATE PROCEDURE [dbo].[Nop_BlogCommentLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_BlogComment]
	ORDER BY CreatedOn
END
