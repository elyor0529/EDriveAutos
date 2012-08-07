

CREATE PROCEDURE [dbo].[Nop_MeasureDimensionLoadByPrimaryKey]
(
	@MeasureDimensionID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_MeasureDimension]
	WHERE
		MeasureDimensionID = @MeasureDimensionID
END
