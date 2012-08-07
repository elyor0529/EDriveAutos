
CREATE PROCEDURE [dbo].[proc_ED_UserMasterLoadAll]
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

	SET @Err = @@Error

	RETURN @Err
END
