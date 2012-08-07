

CREATE PROCEDURE [dbo].[Nop_PollUpdate]
(
	@PollID int,
	@LanguageID int,
	@Name nvarchar(400),
	@SystemKeyword nvarchar(400),
	@Published bit,
	@DisplayOrder int
)
AS
BEGIN
	UPDATE [Nop_Poll]
	SET
	LanguageID=@LanguageID,
	[Name]=@Name,
	SystemKeyword=@SystemKeyword,
	Published=@Published,
	DisplayOrder=@DisplayOrder
	WHERE
		PollID = @PollID
END
