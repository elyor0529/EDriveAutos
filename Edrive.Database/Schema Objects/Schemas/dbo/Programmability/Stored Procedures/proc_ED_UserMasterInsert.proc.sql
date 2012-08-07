
CREATE PROCEDURE [dbo].[proc_ED_UserMasterInsert]
(
	@UserId int = NULL output,
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

	INSERT
	INTO [ED_UserMaster]
	(
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
	)
	VALUES
	(
		@UserType,
		@UserName,
		@Password,
		@FirstName,
		@LastName,
		@IsActive,
		@CreatedOn,
		@CreatedBy,
		@UpdatedBy,
		@UpdatedOn
	)

	SET @Err = @@Error

	SELECT @UserId = SCOPE_IDENTITY()

	RETURN @Err
END
