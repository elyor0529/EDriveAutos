

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLoadByCheckoutAttributeID]
(
	@CheckoutAttributeID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		cav.CheckoutAttributeValueID, 
		cav.CheckoutAttributeID, 
		dbo.NOP_getnotnullnotempty(cavl.Name,cav.Name) as [Name],
		cav.PriceAdjustment, 
		cav.WeightAdjustment, 
		cav.IsPreSelected, 
		cav.DisplayOrder
	FROM [Nop_CheckoutAttributeValue] cav
		LEFT OUTER JOIN [Nop_CheckoutAttributeValueLocalized] cavl 
		ON cav.CheckoutAttributeValueID = cavl.CheckoutAttributeValueID AND cavl.LanguageID = @LanguageID	
	WHERE 
		cav.CheckoutAttributeID=@CheckoutAttributeID
	ORDER BY cav.[DisplayOrder]
END
