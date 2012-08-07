

CREATE PROCEDURE [dbo].[Nop_ProductTemplateUpdate]
(
	@ProductTemplateID int,
	@Name nvarchar(100),
	@TemplatePath nvarchar(200),
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ProductTemplate]
	SET
		[Name]=@Name,
		TemplatePath=@TemplatePath,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn

	WHERE
		ProductTemplateID = @ProductTemplateID
END
