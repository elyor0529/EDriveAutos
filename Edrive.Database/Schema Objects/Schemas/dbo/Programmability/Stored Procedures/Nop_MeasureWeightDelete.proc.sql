

CREATE PROCEDURE [dbo].[Nop_MeasureWeightDelete]
(
	@MeasureWeightID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_MeasureWeight]
	WHERE
		MeasureWeightID = @MeasureWeightID
END
