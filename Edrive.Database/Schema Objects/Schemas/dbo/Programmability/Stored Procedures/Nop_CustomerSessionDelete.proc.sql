

CREATE PROCEDURE [dbo].[Nop_CustomerSessionDelete]
(
	@CustomerSessionGUID uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CustomerSession]
	WHERE
		CustomerSessionGUID = @CustomerSessionGUID
END
