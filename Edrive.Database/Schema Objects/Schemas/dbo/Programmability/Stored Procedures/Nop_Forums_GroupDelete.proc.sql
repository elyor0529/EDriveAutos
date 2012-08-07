

CREATE PROCEDURE [dbo].[Nop_Forums_GroupDelete]
(
	@ForumGroupID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Forums_Group]
	WHERE
		ForumGroupID = @ForumGroupID
END
