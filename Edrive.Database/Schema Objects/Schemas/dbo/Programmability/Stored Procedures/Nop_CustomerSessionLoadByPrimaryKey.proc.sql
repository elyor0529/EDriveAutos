

CREATE PROCEDURE [dbo].[Nop_CustomerSessionLoadByPrimaryKey]
(
	@CustomerSessionGUID uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CustomerSession]
	WHERE
		CustomerSessionGUID =  @CustomerSessionGUID
	order by LastAccessed desc
END
