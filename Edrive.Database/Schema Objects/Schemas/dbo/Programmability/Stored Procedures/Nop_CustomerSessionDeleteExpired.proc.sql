

CREATE PROCEDURE [dbo].[Nop_CustomerSessionDeleteExpired]
(
	@OlderThan datetime
)
AS
BEGIN
	SET NOCOUNT ON
		
	DELETE FROM [Nop_CustomerSession]
	WHERE CustomerSessionGUID IN
	(
		SELECT cs.CustomerSessionGUID
		FROM [Nop_CustomerSession] cs
		WHERE 
			cs.CustomerSessionGUID NOT IN 
				(
					SELECT DISTINCT sci.CustomerSessionGUID FROM [Nop_ShoppingCartItem] sci
				)
			AND
			(
				cs.LastAccessed < @OlderThan
			)
	)
END
