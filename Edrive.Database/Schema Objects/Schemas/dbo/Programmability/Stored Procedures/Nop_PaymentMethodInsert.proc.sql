

CREATE PROCEDURE [dbo].[Nop_PaymentMethodInsert]
(
	@PaymentMethodID int = NULL output,
	@Name nvarchar(100),
	@VisibleName nvarchar(100),
	@Description nvarchar(4000),
	@ConfigureTemplatePath nvarchar(500),
	@UserTemplatePath nvarchar(500),
	@ClassName nvarchar(500),
	@SystemKeyword nvarchar(500),
	@IsActive bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_PaymentMethod]
	(
		[Name],
		VisibleName,
		[Description],
		ConfigureTemplatePath,
		UserTemplatePath,
		ClassName,
		SystemKeyword,
		IsActive,
		DisplayOrder
	)
	VALUES
	(
		@Name,
		@VisibleName,
		@Description,
		@ConfigureTemplatePath,
		@UserTemplatePath,
		@ClassName,
		@SystemKeyword,
		@IsActive,
		@DisplayOrder
	)

	set @PaymentMethodID=SCOPE_IDENTITY()
END
