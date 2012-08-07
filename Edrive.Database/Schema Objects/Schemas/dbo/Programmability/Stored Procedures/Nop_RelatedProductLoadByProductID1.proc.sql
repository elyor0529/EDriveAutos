

CREATE PROCEDURE [dbo].[Nop_RelatedProductLoadByProductID1]
(
	@ProductID1 int,
	@ShowHidden bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		rp.*
	FROM Nop_RelatedProduct rp
	INNER JOIN Nop_Product p ON rp.ProductID2=p.ProductID
	WHERE rp.ProductID1=@ProductID1
		AND (p.Published = 1 or @ShowHidden = 1) and p.Deleted=0
	ORDER BY rp.DisplayOrder
END
