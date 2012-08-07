

CREATE PROCEDURE [dbo].[Nop_QueuedEmailUpdate]
(
	@QueuedEmailID int,
	@Priority int,
	@From nvarchar(500),
	@FromName nvarchar(500),
	@To nvarchar(500),
	@ToName nvarchar(500),
	@Cc nvarchar(500),
	@Bcc nvarchar(500),
	@Subject nvarchar(500),
	@Body nvarchar(MAX),
	@CreatedOn datetime,
	@SendTries int,
	@SentOn datetime
)
AS
BEGIN
UPDATE [Nop_QueuedEmail]
SET
	[Priority]=@Priority,
	[From]=@From,
	[FromName]=@FromName,
	[To]=@To,
	[ToName]=@ToName,
	[Cc]=@Cc,
	[Bcc]=@Bcc,
	[Subject]=@Subject,
	[Body]=@Body,
	[CreatedOn]=@CreatedOn,
	[SendTries]=@SendTries,
	[SentOn]=@SentOn
WHERE
	QueuedEmailID= @QueuedEmailID

END
