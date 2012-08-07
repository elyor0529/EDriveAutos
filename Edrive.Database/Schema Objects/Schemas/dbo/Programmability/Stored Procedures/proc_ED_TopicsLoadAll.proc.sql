
CREATE PROCEDURE [dbo].[proc_ED_TopicsLoadAll]
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

	SET @Err = @@Error

	RETURN @Err
END
