

CREATE PROCEDURE [dbo].[Nop_TaxProviderInsert]
(
	@TaxProviderID int = NULL output,
	@Name nvarchar(100),
	@Description nvarchar(4000),
	@ConfigureTemplatePath nvarchar(500),
	@ClassName nvarchar(500),
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_TaxProvider]
	(
		[Name],
		[Description],
		ConfigureTemplatePath,
		ClassName,
		DisplayOrder
	)
	VALUES
	(
		@Name,
		@Description,
		@ConfigureTemplatePath,
		@ClassName,
		@DisplayOrder
	)

	set @TaxProviderID=SCOPE_IDENTITY()
END
