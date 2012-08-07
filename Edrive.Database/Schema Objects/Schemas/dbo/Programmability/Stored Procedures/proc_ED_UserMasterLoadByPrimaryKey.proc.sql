
CREATE PROCEDURE [dbo].[proc_ED_UserMasterLoadByPrimaryKey]
(
	@UserId int
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[UserId],
		[UserType],
		[UserName],
		[Password],
		[FirstName],
		[LastName],
		[IsActive],
		[CreatedOn],
		[CreatedBy],
		[UpdatedBy],
		[UpdatedOn]
	FROM [ED_UserMaster]
	WHERE
		([UserId] = @UserId)

	SET @Err = @@Error

	RETURN @Err
END
