

CREATE PROCEDURE [dbo].[Nop_CustomerRoleInsert]
(
	@CustomerRoleID int = NULL output,
	@Name nvarchar(255),
	@FreeShipping bit,
	@TaxExempt bit,
	@Active bit,
	@Deleted bit
)
AS
BEGIN
	INSERT
	INTO [Nop_CustomerRole]
	(
		[Name],
		FreeShipping,
		TaxExempt,
		Active,
		Deleted	
	)
	VALUES
	(
		@Name,
		@FreeShipping,
		@TaxExempt,
		@Active,
		@Deleted
	)

	set @CustomerRoleID=SCOPE_IDENTITY()
END
