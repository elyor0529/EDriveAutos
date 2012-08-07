

CREATE PROCEDURE [dbo].[Nop_CategoryTemplateUpdate]
(
	@CategoryTemplateID int,
	@Name nvarchar(100),
	@TemplatePath nvarchar(200),
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_CategoryTemplate]
	SET
		[Name]=@Name,
		TemplatePath=@TemplatePath,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn

	WHERE
		CategoryTemplateID = @CategoryTemplateID
END
