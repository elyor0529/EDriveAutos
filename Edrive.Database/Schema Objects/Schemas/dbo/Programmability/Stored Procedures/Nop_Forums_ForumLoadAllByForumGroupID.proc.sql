

CREATE PROCEDURE [dbo].[Nop_Forums_ForumLoadAllByForumGroupID]
(
	@ForumGroupID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Forums_Forum]
	where ForumGroupID=@ForumGroupID
	order by DisplayOrder
END
