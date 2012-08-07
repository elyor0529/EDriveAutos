-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Get City by id
-- =============================================

CREATE PROCEDURE [dbo].[ED_CityLoadByPrimaryKey]
(
	@CityId int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [ED_City]
	WHERE
		CityId = @CityId
END

