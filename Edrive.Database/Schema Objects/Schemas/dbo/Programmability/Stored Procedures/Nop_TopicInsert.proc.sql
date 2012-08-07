

CREATE PROCEDURE [dbo].[Nop_TopicInsert]
(
	@TopicID int = NULL output,
	@Name nvarchar(200)
)
AS
BEGIN
	INSERT
	INTO [Nop_Topic]
	(
		[Name]
	)
	VALUES
	(
		@Name
	)

	set @TopicID=SCOPE_IDENTITY()
END
