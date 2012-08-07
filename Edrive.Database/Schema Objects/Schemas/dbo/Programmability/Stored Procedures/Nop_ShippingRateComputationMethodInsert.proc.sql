

CREATE PROCEDURE [dbo].[Nop_ShippingRateComputationMethodInsert]
(
	@ShippingRateComputationMethodID int = NULL output,
	@Name nvarchar(100),
	@Description nvarchar(4000),
	@ConfigureTemplatePath nvarchar(500),
	@ClassName nvarchar(500),
	@IsActive bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_ShippingRateComputationMethod]
	(
		[Name],
		[Description],
		ConfigureTemplatePath,
		ClassName,
		IsActive,
		DisplayOrder
	)
	VALUES
	(
		@Name,
		@Description,
		@ConfigureTemplatePath,
		@ClassName,
		@IsActive,
		@DisplayOrder
	)

	set @ShippingRateComputationMethodID=SCOPE_IDENTITY()
END
