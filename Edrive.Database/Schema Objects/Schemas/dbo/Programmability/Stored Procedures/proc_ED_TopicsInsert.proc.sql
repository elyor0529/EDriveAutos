
CREATE PROCEDURE [dbo].[proc_ED_TopicsInsert]
(
	@TopicId tinyint = NULL output,
	@TopicTitle varchar(100),
	@TopicContent nvarchar(MAX) = NULL,
	@CreatedBy int,
	@CreatedOn datetime,
	@UpdatedBy int,
	@UpdatedOn datetime
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	INSERT
	INTO [ED_Topics]
	(
		[TopicTitle],
		[TopicContent],
		[CreatedBy],
		[CreatedOn],
		[UpdatedBy],
		[UpdatedOn]
	)
	VALUES
	(
		@TopicTitle,
		@TopicContent,
		@CreatedBy,
		@CreatedOn,
		@UpdatedBy,
		@UpdatedOn
	)

	SET @Err = @@Error

	SELECT @TopicId = SCOPE_IDENTITY()

	RETURN @Err
END
