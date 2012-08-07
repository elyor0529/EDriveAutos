

CREATE PROCEDURE [dbo].[Nop_QueuedEmailLoadByPrimaryKey]
(
	@QueuedEmailID int
)
AS
BEGIN
SET NOCOUNT ON
SELECT
*
FROM [Nop_QueuedEmail]
WHERE
	QueuedEmailID=@QueuedEmailID
END
