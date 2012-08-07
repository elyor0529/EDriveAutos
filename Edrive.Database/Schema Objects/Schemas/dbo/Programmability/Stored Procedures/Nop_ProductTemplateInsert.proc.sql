

CREATE PROCEDURE [dbo].[Nop_ProductTemplateInsert]
(
	@ProductTemplateID int = NULL output,
	@Name nvarchar(100),
	@TemplatePath nvarchar(200),
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductTemplate]
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

	set @ProductTemplateID=SCOPE_IDENTITY()
END
