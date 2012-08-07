

CREATE PROCEDURE [dbo].[Nop_CustomerAttributeUpdate]
(
	@CustomerAttributeID int,
	@CustomerID int,
	@Key nvarchar(100),
	@Value nvarchar(1000)
)
AS
BEGIN
	UPDATE [Nop_CustomerAttribute]
	SET
	CustomerID=@CustomerID,
	[Key]=@Key,
	[Value]=@Value
	WHERE
		CustomerAttributeID = @CustomerAttributeID
END
