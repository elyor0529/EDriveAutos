

CREATE PROCEDURE [dbo].[Nop_CustomerRegisteredReport]
(
	@Date datetime
)
AS
BEGIN

	SELECT COUNT(CustomerID) [Count]
	FROM Nop_Customer
	WHERE 
		Active = 1 
		AND Deleted = 0
		AND IsGuest = 0
		AND RegistrationDate BETWEEN @Date AND GETUTCDATE()

END
