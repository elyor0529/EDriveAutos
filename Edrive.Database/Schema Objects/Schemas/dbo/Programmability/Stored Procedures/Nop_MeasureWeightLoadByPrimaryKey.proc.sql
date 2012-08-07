

CREATE PROCEDURE [dbo].[Nop_MeasureWeightLoadByPrimaryKey]
(
	@MeasureWeightID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_MeasureWeight]
	WHERE
		MeasureWeightID = @MeasureWeightID
END
