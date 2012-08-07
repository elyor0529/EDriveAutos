

CREATE PROCEDURE [dbo].[Nop_PollDelete]
(
	@PollID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Poll]
	WHERE
		PollID = @PollID
END
