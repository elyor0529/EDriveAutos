

CREATE PROCEDURE [dbo].[Nop_PollVotingRecordCreate]
(
	@PollAnswerID int,
	@CustomerID int
)
AS
BEGIN

	DELETE FROM Nop_PollVotingRecord WHERE PollAnswerID=@PollAnswerID AND CustomerID=@CustomerID
	
	INSERT
	INTO [Nop_PollVotingRecord]
	(
		PollAnswerID,
		CustomerID
	)
	VALUES
	(
		@PollAnswerID,
		@CustomerID
	)
	
	DECLARE @TotalVotingRecords int
	SELECT @TotalVotingRecords = COUNT(*) FROM [Nop_PollVotingRecord] WHERE PollAnswerID=@PollAnswerID 
	UPDATE Nop_PollAnswer SET [Count]=@TotalVotingRecords WHERE PollAnswerID=@PollAnswerID
	


END
