-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Delete City
-- =============================================

CREATE PROCEDURE [dbo].[ED_CityDelete]
(
	@CityId int
)
AS
BEGIN
	SET NOCOUNT ON
	update [ED_City]
	set Deleted = 1
	WHERE
		CityId = @CityId
END