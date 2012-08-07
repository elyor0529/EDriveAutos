

CREATE PROCEDURE [dbo].[Nop_PollAnswerDelete]
(
	@PollAnswerID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_PollAnswer]
	WHERE
		PollAnswerID = @PollAnswerID
END
