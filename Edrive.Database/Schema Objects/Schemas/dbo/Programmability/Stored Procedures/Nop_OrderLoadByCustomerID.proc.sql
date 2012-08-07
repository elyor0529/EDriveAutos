

CREATE PROCEDURE [dbo].[Nop_OrderLoadByCustomerID]
(
	@CustomerID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Order]
	WHERE
		CustomerID=@CustomerID and Deleted=0
	ORDER BY CreatedOn desc
END
