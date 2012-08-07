

CREATE PROCEDURE [dbo].[Nop_QueuedEmailDelete]
(
	@QueuedEmailID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_QueuedEmail]
	WHERE
		[QueuedEmailID] = @QueuedEmailID
END
