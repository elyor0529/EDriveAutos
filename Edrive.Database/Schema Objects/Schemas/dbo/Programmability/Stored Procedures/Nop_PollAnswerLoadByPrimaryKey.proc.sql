

CREATE PROCEDURE [dbo].[Nop_PollAnswerLoadByPrimaryKey]
(
	@PollAnswerID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_PollAnswer]
	WHERE
		PollAnswerID = @PollAnswerID
END
