

CREATE PROCEDURE [dbo].[Nop_ProductVariant_ProductAttribute_MappingUpdate]
(
	@ProductVariantAttributeID int,
	@ProductVariantID int,
	@ProductAttributeID int,
	@TextPrompt nvarchar(200),
	@IsRequired bit,
	@AttributeControlTypeID int,
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_ProductVariant_ProductAttribute_Mapping]
	SET
		ProductVariantID=@ProductVariantID,
		ProductAttributeID=@ProductAttributeID,
		TextPrompt=@TextPrompt,
		IsRequired=@IsRequired,
		AttributeControlTypeID=@AttributeControlTypeID,
		DisplayOrder=@DisplayOrder
	WHERE
		ProductVariantAttributeID = @ProductVariantAttributeID
END
