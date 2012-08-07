

CREATE PROCEDURE [dbo].[Nop_CustomerActionInsert]
(
	@CustomerActionID int = NULL output,
	@Name nvarchar(100),
	@SystemKeyword nvarchar(100),
	@Comment nvarchar(1000),
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_CustomerAction]
	(
		[Name],
		[SystemKeyword],
		[Comment],
		[DisplayOrder]
	)
	VALUES
	(
		@Name,
		@SystemKeyword,
		@Comment,
		@DisplayOrder
	)

	set @CustomerActionID=SCOPE_IDENTITY()
END
