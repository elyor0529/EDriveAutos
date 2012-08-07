

CREATE PROCEDURE [dbo].[Nop_DiscountLoadByCategoryID]
(
	@CategoryID int,
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		d.*
	FROM [Nop_Discount] d
	INNER JOIN Nop_Category_Discount_Mapping cdm
	ON d.DiscountID = cdm.DiscountID
	WHERE ((getutcdate() between d.StartDate and d.EndDate) or @ShowHidden = 1) and d.Deleted=0 and cdm.CategoryID=@CategoryID
	order by d.StartDate desc
END
