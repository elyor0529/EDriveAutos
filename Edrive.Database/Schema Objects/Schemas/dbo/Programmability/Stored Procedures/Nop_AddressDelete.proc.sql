

CREATE PROCEDURE [dbo].[Nop_AddressDelete]
(
	@AddressID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Address]
	WHERE
		AddressID = @AddressID
END
