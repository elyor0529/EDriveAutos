-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Get FAQ by id
-- =============================================
create PROCEDURE [dbo].[ED_CheckCityName]
(
	@CityName nvarchar(max)
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT * 
	FROM ED_City
	WHERE
		Name =@CityName

	SET @Err = @@Error

	RETURN @Err
END