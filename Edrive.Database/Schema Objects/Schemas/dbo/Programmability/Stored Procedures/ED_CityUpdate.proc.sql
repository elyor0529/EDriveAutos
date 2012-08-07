-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Update City
-- =============================================

CREATE PROCEDURE [dbo].[ED_CityUpdate]
(
	@CityId int,
	@CountryID int,
	@StateProvinceID int,
	@Name nvarchar(100),
	@CityImageId int,
	@DisplayOrder int,
	@UpdatedOn datetime,
	@ShowOnHomepage bit
)
AS
BEGIN
	UPDATE [ED_City]
	SET
		CountryID=@CountryID,
		StateProvinceID=@StateProvinceID,
		[Name]=@Name,
		CityImageId=@CityImageId,
		DisplayOrder=@DisplayOrder,
		UpdatedOn=@UpdatedOn,
		ShowOnHomepage=@ShowOnHomepage
	WHERE
		CityId = @CityId
END