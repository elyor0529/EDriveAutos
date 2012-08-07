
CREATE PROCEDURE [dbo].[proc_ED_TopicsUpdate]
(
	@TopicId tinyint,
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

	UPDATE [ED_Topics]
	SET
		[TopicTitle] = @TopicTitle,
		[TopicContent] = @TopicContent,
		[CreatedBy] = @CreatedBy,
		[CreatedOn] = @CreatedOn,
		[UpdatedBy] = @UpdatedBy,
		[UpdatedOn] = @UpdatedOn
	WHERE
		[TopicId] = @TopicId


	SET @Err = @@Error


	RETURN @Err
END
