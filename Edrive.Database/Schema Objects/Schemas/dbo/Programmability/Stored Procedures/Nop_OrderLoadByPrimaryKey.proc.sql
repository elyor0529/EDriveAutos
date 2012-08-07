

CREATE PROCEDURE [dbo].[Nop_OrderLoadByPrimaryKey]
(
	@OrderID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Order]
	WHERE
		OrderID=@OrderID
END
