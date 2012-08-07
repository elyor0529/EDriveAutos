

CREATE PROCEDURE [dbo].[Nop_NewsCommentLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_NewsComment]
	order by CreatedOn
END
