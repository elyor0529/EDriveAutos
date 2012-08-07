

CREATE PROCEDURE [dbo].[Nop_ProductPictureLoadAllByProductID]
(
	@ProductID int,
	@PictureCount int
)
AS
BEGIN
	IF(@PictureCount > 0) SET ROWCOUNT @PictureCount
	
	SELECT
		*
	FROM 
		Nop_ProductPicture
	WHERE 
		ProductID = @ProductID
	ORDER BY 
		DisplayOrder
		
	SET ROWCOUNT 0
END
