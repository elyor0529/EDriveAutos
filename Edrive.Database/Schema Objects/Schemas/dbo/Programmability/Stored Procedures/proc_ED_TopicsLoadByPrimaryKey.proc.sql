
CREATE PROCEDURE [dbo].[proc_ED_TopicsLoadByPrimaryKey]
(
	@TopicId tinyint
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[TopicId],
		[TopicTitle],
		[TopicContent],
		[CreatedBy],
		[CreatedOn],
		[UpdatedBy],
		[UpdatedOn]
	FROM [ED_Topics]
	WHERE
		([TopicId] = @TopicId)

	SET @Err = @@Error

	RETURN @Err
END
