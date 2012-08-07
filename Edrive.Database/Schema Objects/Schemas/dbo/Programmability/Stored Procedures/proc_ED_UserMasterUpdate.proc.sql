
CREATE PROCEDURE [dbo].[proc_ED_UserMasterUpdate]
(
	@UserId int,
	@UserType tinyint,
	@UserName varchar(50),
	@Password varchar(50),
	@FirstName varchar(100),
	@LastName varchar(100) = NULL,
	@IsActive bit,
	@CreatedOn datetime = NULL,
	@CreatedBy int = NULL,
	@UpdatedBy int = NULL,
	@UpdatedOn datetime = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [ED_UserMaster]
	SET
		[UserType] = @UserType,
		[UserName] = @UserName,
		[Password] = @Password,
		[FirstName] = @FirstName,
		[LastName] = @LastName,
		[IsActive] = @IsActive,
		[CreatedOn] = @CreatedOn,
		[CreatedBy] = @CreatedBy,
		[UpdatedBy] = @UpdatedBy,
		[UpdatedOn] = @UpdatedOn
	WHERE
		[UserId] = @UserId


	SET @Err = @@Error


	RETURN @Err
END
