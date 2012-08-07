

CREATE PROCEDURE [dbo].[Nop_ProductVariant_ProductAttribute_MappingInsert]
(
	@ProductVariantAttributeID int = NULL output,
	@ProductVariantID int,
	@ProductAttributeID int,
	@TextPrompt nvarchar(200),
	@IsRequired bit,
	@AttributeControlTypeID int,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductVariant_ProductAttribute_Mapping]
	(
		ProductVariantID,
		ProductAttributeID,
		TextPrompt,
		IsRequired,
		AttributeControlTypeID,
		DisplayOrder
	)
	VALUES
	(
		@ProductVariantID,
		@ProductAttributeID,
		@TextPrompt,
		@IsRequired,
		@AttributeControlTypeID,
		@DisplayOrder
	)

	set @ProductVariantAttributeID=SCOPE_IDENTITY()
END
