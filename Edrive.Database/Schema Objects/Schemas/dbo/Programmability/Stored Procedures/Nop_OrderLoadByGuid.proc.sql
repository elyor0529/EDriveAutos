

CREATE PROCEDURE [dbo].[Nop_OrderLoadByGuid]
(
	@OrderGUID uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Order]
	WHERE
		[OrderGUID] = @OrderGUID
END
