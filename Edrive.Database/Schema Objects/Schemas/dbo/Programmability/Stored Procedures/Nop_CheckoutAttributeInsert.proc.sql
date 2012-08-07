

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeInsert]
(
	@CheckoutAttributeID int = NULL output,
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
	INSERT
	INTO [Nop_CheckoutAttribute]
	(
		[Name],
		[TextPrompt],
		[IsRequired],
		[ShippableProductRequired],
		[IsTaxExempt],
		[TaxCategoryID],
		[AttributeControlTypeID],
		[DisplayOrder]
	)
	VALUES
	(
		@Name,
		@TextPrompt,
		@IsRequired,
		@ShippableProductRequired,
		@IsTaxExempt,
		@TaxCategoryID,
		@AttributeControlTypeID,
		@DisplayOrder
	)

	set @CheckoutAttributeID=SCOPE_IDENTITY()
END
