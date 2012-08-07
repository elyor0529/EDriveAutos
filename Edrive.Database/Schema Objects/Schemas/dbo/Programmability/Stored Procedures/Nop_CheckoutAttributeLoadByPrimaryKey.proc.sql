

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLoadByPrimaryKey]
(
	@CheckoutAttributeID int,
	@LanguageID int
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
	WHERE
		ca.CheckoutAttributeID = @CheckoutAttributeID
END
