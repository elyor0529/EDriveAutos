

CREATE PROCEDURE [dbo].[Nop_TaxCategoryInsert]
(
	@TaxCategoryID int = NULL output,
	@Name nvarchar(100),
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_TaxCategory]
	(
		[Name],
		DisplayOrder,
		CreatedOn,
		UpdatedOn	
	)
	VALUES
	(
		@Name,
		@DisplayOrder,
		@CreatedOn,
		@UpdatedOn
	)

	set @TaxCategoryID=SCOPE_IDENTITY()
END
