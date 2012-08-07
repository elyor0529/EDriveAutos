

CREATE PROCEDURE [dbo].[Nop_PollInsert]
(
	@PollID int = NULL output,
	@LanguageID int,
	@Name nvarchar(400),
	@SystemKeyword nvarchar(400),
	@Published bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_Poll]
	(
		LanguageID,
		[Name],
		SystemKeyword,
		Published,
		DisplayOrder		
	)
	VALUES
	(
		@LanguageID,
		@Name,
		@SystemKeyword,
		@Published,
		@DisplayOrder
	)

	set @PollID=SCOPE_IDENTITY()
END
