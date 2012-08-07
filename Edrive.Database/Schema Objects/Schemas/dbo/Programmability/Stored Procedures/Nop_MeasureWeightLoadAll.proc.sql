

CREATE PROCEDURE [dbo].[Nop_MeasureWeightLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_MeasureWeight]
	order by [DisplayOrder]
END
