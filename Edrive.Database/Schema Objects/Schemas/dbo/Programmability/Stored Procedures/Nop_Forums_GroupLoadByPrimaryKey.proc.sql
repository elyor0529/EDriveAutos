

CREATE PROCEDURE [dbo].[Nop_Forums_GroupLoadByPrimaryKey]
(
	@ForumGroupID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_Group]
	WHERE
		(ForumGroupID = @ForumGroupID)
END
