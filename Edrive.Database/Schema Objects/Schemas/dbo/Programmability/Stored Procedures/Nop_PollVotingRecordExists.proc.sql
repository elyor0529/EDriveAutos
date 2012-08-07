

CREATE PROCEDURE [dbo].[Nop_PollVotingRecordExists]
(
	@PollID int,
	@CustomerID int
)
AS
BEGIN
	SELECT COUNT(1) FROM Nop_PollVotingRecord pvr
	INNER JOIN Nop_PollAnswer pa ON pvr.PollAnswerID = pa.PollAnswerID
    WHERE pa.PollID=@PollID AND pvr.CustomerID=@CustomerID	
END
