

CREATE PROCEDURE [dbo].[Nop_Product_Category_MappingLoadByCategoryID]
(
	@CategoryID int,
	@ShowHidden bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		pcm.*
	FROM Nop_Product_Category_Mapping pcm
	INNER JOIN Nop_Product p ON pcm.ProductID=p.ProductID
	WHERE pcm.CategoryID=@CategoryID
		AND (p.Published = 1 or @ShowHidden = 1) and p.Deleted=0
	ORDER BY pcm.DisplayOrder
END
