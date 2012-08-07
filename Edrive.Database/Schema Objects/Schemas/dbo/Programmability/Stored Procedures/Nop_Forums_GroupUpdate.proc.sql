

CREATE PROCEDURE [dbo].[Nop_Forums_GroupUpdate]
(
	@ForumGroupID int,
	@Name nvarchar(200),
	@Description nvarchar(MAX),
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_Group]
	SET
		[Name]=@Name,
		[Description]=@Description,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		ForumGroupID = @ForumGroupID
END
