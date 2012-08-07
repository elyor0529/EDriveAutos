

CREATE PROCEDURE [dbo].[Nop_TaxCategoryUpdate]
(
	@TaxCategoryID int,
	@Name nvarchar(100),
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_TaxCategory]
	SET
		[Name]=@Name,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn

	WHERE
		TaxCategoryID = @TaxCategoryID
END
