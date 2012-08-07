
CREATE PROCEDURE [dbo].[proc_ED_UserMasterDelete]
(
	@UserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DELETE
	FROM [ED_UserMaster]
	WHERE
		[UserId] = @UserId
	SET @Err = @@Error

	RETURN @Err
END
