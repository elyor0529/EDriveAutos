

CREATE PROCEDURE [dbo].[Nop_StateProvinceUpdate]
(
	@StateProvinceID int,
	@CountryID int,
	@Name nvarchar(100),
	@Abbreviation nvarchar (10),
	@DisplayOrder int
)
AS
BEGIN
	UPDATE [Nop_StateProvince]
	SET
		CountryID=@CountryID,
		[Name]=@Name,
		Abbreviation=@Abbreviation,
		DisplayOrder=@DisplayOrder
	WHERE
		StateProvinceID = @StateProvinceID
END
