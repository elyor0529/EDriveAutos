

CREATE PROCEDURE [dbo].[Nop_PollAnswerInsert]
(
	@PollAnswerID int = NULL output,
	@PollID int,
	@Name nvarchar(100),
	@Count int,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_PollAnswer]
	(
		PollID,
		[Name],
		[Count],
		DisplayOrder
	)
	VALUES
	(
		@PollID,
		@Name,
		@Count,
		@DisplayOrder
	)

	set @PollAnswerID=SCOPE_IDENTITY()
END
