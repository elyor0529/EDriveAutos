

CREATE PROCEDURE [dbo].[Nop_AddressLoadByPrimaryKey]
(
	@AddressId int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Address]
	WHERE
		([AddressId] = @AddressId)
END
