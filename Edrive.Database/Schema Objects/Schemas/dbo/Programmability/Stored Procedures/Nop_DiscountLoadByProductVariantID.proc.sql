

CREATE PROCEDURE [dbo].[Nop_DiscountLoadByProductVariantID]
(
	@ProductVariantID int,
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		d.*
	FROM [Nop_Discount] d
	INNER JOIN Nop_ProductVariant_Discount_Mapping pdm
	ON d.DiscountID = pdm.DiscountID
	WHERE ((getutcdate() between d.StartDate and d.EndDate) or @ShowHidden = 1) and d.Deleted=0 and pdm.ProductVariantID=@ProductVariantID
	order by d.StartDate desc
END
