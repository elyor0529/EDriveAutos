

CREATE PROCEDURE [dbo].[Nop_CategoryTemplateInsert]
(
	@CategoryTemplateID int = NULL output,
	@Name nvarchar(100),
	@TemplatePath nvarchar(200),
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_CategoryTemplate]
	(
		[Name],
		TemplatePath,
		DisplayOrder,
		CreatedOn,
		UpdatedOn	
	)
	VALUES
	(
		@Name,
		@TemplatePath,
		@DisplayOrder,
		@CreatedOn,
		@UpdatedOn
	)

	set @CategoryTemplateID=SCOPE_IDENTITY()
END
