

CREATE PROCEDURE [dbo].[Nop_MeasureWeightUpdate]
(
	@MeasureWeightID int,
	@Name nvarchar(100),
	@SystemKeyword nvarchar(100),
	@Ratio decimal(18, 4),
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_MeasureWeight]
	SET
		[Name]=@Name,
		[SystemKeyword]=@SystemKeyword,
		[Ratio]=@Ratio,
		[DisplayOrder]=@DisplayOrder
	WHERE
		MeasureWeightID = @MeasureWeightID

END
