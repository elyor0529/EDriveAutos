

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeUpdate]
(
	@CheckoutAttributeID int,
	@Name nvarchar(100),
	@TextPrompt nvarchar(300),
	@IsRequired bit,
	@ShippableProductRequired bit,
	@IsTaxExempt bit,
	@TaxCategoryID int,
	@AttributeControlTypeID int,
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_CheckoutAttribute]
	SET
		[Name]=@Name,
		[TextPrompt]=@TextPrompt,
		[IsRequired]=@IsRequired,
		[ShippableProductRequired]=@ShippableProductRequired,
		[IsTaxExempt]=@IsTaxExempt,
		[TaxCategoryID]=@TaxCategoryID,
		[AttributeControlTypeID]=@AttributeControlTypeID,
		[DisplayOrder]=@DisplayOrder
	WHERE
		CheckoutAttributeID = @CheckoutAttributeID
END
