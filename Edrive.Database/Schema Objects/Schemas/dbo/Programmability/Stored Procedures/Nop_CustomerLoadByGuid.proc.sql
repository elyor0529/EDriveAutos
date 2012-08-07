

CREATE PROCEDURE [dbo].[Nop_CustomerLoadByGuid]
(
	@CustomerGUID uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Customer]
	WHERE
		([CustomerGUID] = @CustomerGUID)
END
