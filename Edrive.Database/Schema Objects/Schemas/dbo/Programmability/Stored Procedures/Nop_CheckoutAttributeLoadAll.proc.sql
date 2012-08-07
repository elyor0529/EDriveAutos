

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLoadAll]
(
	@LanguageID int,
	@DontLoadShippableProductRequired bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		ca.[CheckoutAttributeID],
		dbo.NOP_getnotnullnotempty(cal.Name,ca.Name) as [Name],
		dbo.NOP_getnotnullnotempty(cal.TextPrompt,ca.TextPrompt) as [TextPrompt],
		ca.[IsRequired],
		ca.[ShippableProductRequired],
		ca.[IsTaxExempt],
		ca.[TaxCategoryID],
		ca.[AttributeControlTypeID],
		ca.[DisplayOrder]		
	FROM [Nop_CheckoutAttribute] ca
		LEFT OUTER JOIN [Nop_CheckoutAttributeLocalized] cal
		ON ca.CheckoutAttributeID = cal.CheckoutAttributeID AND cal.LanguageID = @LanguageID	
	WHERE (@DontLoadShippableProductRequired=0 OR ca.[ShippableProductRequired]=0)
	order by ca.[DisplayOrder]
END
