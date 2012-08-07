

CREATE PROCEDURE [dbo].[Nop_StateProvinceInsert]
(
	@StateProvinceID int = NULL output,
	@CountryID int,
	@Name nvarchar(100),
	@Abbreviation nvarchar (10),
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_StateProvince]
	(
		CountryID,
		[Name],
		Abbreviation,
		[DisplayOrder]
	)
	VALUES
	(
		@CountryID,
		@Name,
		@Abbreviation,
		@DisplayOrder
	)

	set @StateProvinceID=SCOPE_IDENTITY()
END
