

CREATE PROCEDURE [dbo].[Nop_ProductTypeLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ProductType]
	order by DisplayOrder
END
