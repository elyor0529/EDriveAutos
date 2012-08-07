

CREATE PROCEDURE [dbo].[Nop_ProductVariantLoadBySKU]
(
	@SKU nvarchar (400)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductVariant] pv
	WHERE 
		pv.Deleted=0 AND 
		pv.SKU = @SKU
	order by DisplayOrder
END
