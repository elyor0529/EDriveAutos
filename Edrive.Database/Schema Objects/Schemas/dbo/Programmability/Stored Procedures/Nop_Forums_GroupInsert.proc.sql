

CREATE PROCEDURE [dbo].[Nop_Forums_GroupInsert]
(
	@ForumGroupID int = NULL output,
	@Name nvarchar(200),
	@Description nvarchar(MAX),
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Forums_Group]
	(
		[Name],
		[Description],
		DisplayOrder,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@Name,
		@Description,
		@DisplayOrder,
		@CreatedOn,
		@UpdatedOn
	)

	set @ForumGroupID=SCOPE_IDENTITY()
END
