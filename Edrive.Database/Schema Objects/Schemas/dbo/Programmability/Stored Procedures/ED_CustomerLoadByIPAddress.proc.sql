-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <24/3/2011>
-- Description:	Get Customer By IP Address
-- =============================================

CREATE PROCEDURE [dbo].[ED_CustomerLoadByIPAddress]
(
	@IPAddress nvarchar(100)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		CustomerID
	FROM [Nop_Customer]
	WHERE
		([IPAddress] = @IPAddress)
		And CustomerType=2
		And Deleted=0
	ORDER BY CustomerID
END