

CREATE PROCEDURE [dbo].[Nop_MeasureDimensionUpdate]
(
	@MeasureDimensionID int,
	@Name nvarchar(100),
	@SystemKeyword nvarchar(100),
	@Ratio decimal(18, 4),
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_MeasureDimension]
	SET
		[Name]=@Name,
		[SystemKeyword]=@SystemKeyword,
		[Ratio]=@Ratio,
		[DisplayOrder]=@DisplayOrder
	WHERE
		MeasureDimensionID = @MeasureDimensionID

END
