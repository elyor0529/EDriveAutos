



CREATE PROCEDURE [dbo].[Nop_ProductTagUpdateCounts]
(
	@ProductTagID int
)
AS
BEGIN

	DECLARE @NumRecords int

	SELECT 
		@NumRecords = COUNT(1)
	FROM
		[Nop_ProductTag_Product_Mapping] ptpm
	WHERE
		ptpm.ProductTagID = @ProductTagID

	SET @NumRecords = isnull(@NumRecords, 0)
	
	SET NOCOUNT ON
	UPDATE 
		[Nop_ProductTag]
	SET
		[ProductCount] = @NumRecords
	WHERE
		[ProductTagID] = @ProductTagID
END
