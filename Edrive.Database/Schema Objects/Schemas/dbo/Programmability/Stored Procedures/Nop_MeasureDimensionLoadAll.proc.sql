

CREATE PROCEDURE [dbo].[Nop_MeasureDimensionLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_MeasureDimension]
	order by [DisplayOrder]
END
