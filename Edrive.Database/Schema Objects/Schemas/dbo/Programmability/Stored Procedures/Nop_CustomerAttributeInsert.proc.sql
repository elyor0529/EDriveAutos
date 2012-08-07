

CREATE PROCEDURE [dbo].[Nop_CustomerAttributeInsert]
(
	@CustomerAttributeID int = NULL output,
	@CustomerID int,
	@Key nvarchar(100),
	@Value nvarchar(1000)
)
AS
BEGIN
	INSERT
	INTO [Nop_CustomerAttribute]
	(
		CustomerID,
		[Key],
		[Value]
	)
	VALUES
	(
		@CustomerID,
		@Key,
		@Value
	)

	set @CustomerAttributeID=SCOPE_IDENTITY()
END
