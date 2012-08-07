

CREATE PROCEDURE [dbo].[Nop_PollAnswerUpdate]
(
	@PollAnswerID int,
	@PollID int,
	@Name nvarchar(100),
	@Count int,
	@DisplayOrder int
)
AS
BEGIN
	UPDATE [Nop_PollAnswer]
	SET
	PollID=@PollID,
	[Name]=@Name,
	[Count]=@Count,
	DisplayOrder=@DisplayOrder
	WHERE
		PollAnswerID = @PollAnswerID
END
