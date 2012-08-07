

CREATE PROCEDURE [dbo].[Nop_PaymentMethodUpdate]
(
	@PaymentMethodID int,
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
	UPDATE [Nop_PaymentMethod]
	SET
		[Name]=@Name,
		[Description]=@Description,
		VisibleName=@VisibleName,
		ConfigureTemplatePath=@ConfigureTemplatePath,
		UserTemplatePath=@UserTemplatePath,
		ClassName=@ClassName,
		SystemKeyword=@SystemKeyword,
		IsActive=@IsActive,
		DisplayOrder=@DisplayOrder

	WHERE
		PaymentMethodID = @PaymentMethodID
END
