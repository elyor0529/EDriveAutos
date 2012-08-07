

CREATE PROCEDURE [dbo].[Nop_Product_Category_MappingLoadByProductID]
(
	@ProductID int,
	@ShowHidden bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		pcm.*
	FROM Nop_Product_Category_Mapping pcm
	INNER JOIN Nop_Category c ON pcm.CategoryID=c.CategoryID	
	WHERE pcm.ProductID=@ProductID
		AND (c.Published = 1 or @ShowHidden = 1) and c.Deleted=0
	ORDER BY pcm.DisplayOrder
END
