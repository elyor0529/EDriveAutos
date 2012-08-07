

CREATE PROCEDURE [dbo].[Nop_TaxProviderUpdate]
(
	@TaxProviderID int,
	@Name nvarchar(100),
	@Description nvarchar(4000),
	@ConfigureTemplatePath nvarchar(500),
	@ClassName nvarchar(500),
	@DisplayOrder int
)
AS
BEGIN
	UPDATE [Nop_TaxProvider]
	SET
		[Name]=@Name,
		[Description]=@Description,
		ConfigureTemplatePath=@ConfigureTemplatePath,
		ClassName=@ClassName,
		DisplayOrder=@DisplayOrder

	WHERE
		TaxProviderID = @TaxProviderID
END
