-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Insert City
-- =============================================

CREATE PROCEDURE [dbo].[ED_CityInsert]
(
	@CityId int = NULL output,
	@CountryID int,
	@StateProvinceID int,
	@Name nvarchar(100),
	@CityImageId int,
	@DisplayOrder int,
	@CreatedOn datetime,
	@Deleted bit,
	@ShowOnHomepage bit
)
AS
BEGIN
	INSERT
	INTO [ED_City]
	(
		CountryID,
		StateProvinceID,
		[Name],
		CityImageId,
		[DisplayOrder],
		CreatedOn,
		Deleted,
		ShowOnHomepage
	)
	VALUES
	(
		@CountryID,
		@StateProvinceID,
		@Name,
		@CityImageId,
		@DisplayOrder,
		@CreatedOn,
		@Deleted,
		@ShowOnHomepage
	)

	set @CityId=SCOPE_IDENTITY()
END