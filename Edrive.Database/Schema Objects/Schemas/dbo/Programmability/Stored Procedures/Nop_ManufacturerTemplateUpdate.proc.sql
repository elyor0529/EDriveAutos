

CREATE PROCEDURE [dbo].[Nop_ManufacturerTemplateUpdate]
(
	@ManufacturerTemplateID int,
	@Name nvarchar(100),
	@TemplatePath nvarchar(200),
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ManufacturerTemplate]
	SET
		[Name]=@Name,
		TemplatePath=@TemplatePath,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn

	WHERE
		ManufacturerTemplateID = @ManufacturerTemplateID
END
