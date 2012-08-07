

CREATE PROCEDURE [dbo].[Nop_Forums_ForumLoadByPrimaryKey]
(
	@ForumID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_Forum]
	WHERE
		(ForumID = @ForumID)
END
