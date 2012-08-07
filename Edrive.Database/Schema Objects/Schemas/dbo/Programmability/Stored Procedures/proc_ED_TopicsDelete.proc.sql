
CREATE PROCEDURE [dbo].[proc_ED_TopicsDelete]
(
	@TopicId tinyint
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DELETE
	FROM [ED_Topics]
	WHERE
		[TopicId] = @TopicId
	SET @Err = @@Error

	RETURN @Err
END
