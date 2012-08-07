

CREATE PROCEDURE [dbo].[Nop_PollAnswerLoadByPollID]
(
	@PollID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_PollAnswer]
	WHERE
		PollID=@PollID
	ORDER BY DisplayOrder
END
