

CREATE PROCEDURE [dbo].[Nop_Forums_PostLoadByPrimaryKey]
(
	@PostID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_Post]
	WHERE
		(PostID = @PostID)
END
