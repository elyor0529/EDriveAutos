

CREATE PROCEDURE [dbo].[Nop_QueuedEmailLoadAll]
(	
	@FromEmail nvarchar(255) = NULL,
	@ToEmail nvarchar(255) = NULL,
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@QueuedEmailCount int,
	@LoadNotSentItemsOnly bit,
	@MaxSendTries int
)
AS
BEGIN
	IF (@QueuedEmailCount > 0)
	SET ROWCOUNT @QueuedEmailCount

	SELECT qu.*
	FROM [Nop_QueuedEmail] qu
	WHERE 
		(@FromEmail IS NULL or LEN(@FromEmail)=0 or (qu.[From] like '%' + COALESCE(@FromEmail,qu.[From]) + '%')) AND
		(@ToEmail IS NULL or LEN(@ToEmail)=0 or (qu.[To] like '%' + COALESCE(@ToEmail,qu.[To]) + '%')) AND
		(@StartTime is NULL or @StartTime <= qu.CreatedOn) AND
		(@EndTime is NULL or @EndTime >= qu.CreatedOn) AND 
		((@LoadNotSentItemsOnly IS NULL OR @LoadNotSentItemsOnly=0) OR (qu.SentOn IS NULL)) AND
		(qu.SendTries < @MaxSendTries)
	ORDER BY qu.Priority desc, qu.CreatedOn ASC
	
	SET ROWCOUNT 0
END
