-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Get City by id
-- =============================================

CREATE PROCEDURE [dbo].[ED_InterestedCarLoadByType]
(
	@InterestType int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [ED_InterestedCustomer]
	WHERE
		InterestType = @InterestType
		and IsActive = 1
END

