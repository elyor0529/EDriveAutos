

CREATE PROCEDURE [dbo].[Nop_MeasureDimensionDelete]
(
	@MeasureDimensionID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_MeasureDimension]
	WHERE
		MeasureDimensionID = @MeasureDimensionID
END
