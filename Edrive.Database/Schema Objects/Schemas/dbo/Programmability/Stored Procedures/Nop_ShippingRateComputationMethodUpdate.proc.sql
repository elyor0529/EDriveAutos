

CREATE PROCEDURE [dbo].[Nop_ShippingRateComputationMethodUpdate]
(
	@ShippingRateComputationMethodID int,
	@Name nvarchar(100),
	@Description nvarchar(4000),
	@ConfigureTemplatePath nvarchar(500),
	@ClassName nvarchar(500),
	@IsActive bit,
	@DisplayOrder int
)
AS
BEGIN
	UPDATE [Nop_ShippingRateComputationMethod]
	SET
		[Name]=@Name,
		[Description]=@Description,
		ConfigureTemplatePath=@ConfigureTemplatePath,
		ClassName=@ClassName,
		IsActive=@IsActive,
		DisplayOrder=@DisplayOrder

	WHERE
		ShippingRateComputationMethodID = @ShippingRateComputationMethodID
END
