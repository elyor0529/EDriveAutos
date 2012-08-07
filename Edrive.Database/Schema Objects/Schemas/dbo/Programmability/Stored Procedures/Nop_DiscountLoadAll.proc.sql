

CREATE PROCEDURE [dbo].[Nop_DiscountLoadAll]
(
	@ShowHidden bit = 0,	
	@DiscountTypeID int		/*null or 0 to load all discounts*/
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Discount] d
	WHERE ((getutcdate() between d.StartDate and d.EndDate) or @ShowHidden = 1)
	and (@DiscountTypeID IS NULL or @DiscountTypeID=0 or d.DiscountTypeID = @DiscountTypeID) 
	and d.Deleted=0
	order by d.StartDate desc
END
