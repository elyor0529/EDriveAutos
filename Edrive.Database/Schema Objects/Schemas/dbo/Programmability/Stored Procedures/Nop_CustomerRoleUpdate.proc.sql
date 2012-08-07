

CREATE PROCEDURE [dbo].[Nop_CustomerRoleUpdate]
(
	@CustomerRoleID int,
	@Name nvarchar(255),
	@FreeShipping bit,
	@TaxExempt bit,
	@Active bit,
	@Deleted bit
)
AS
BEGIN
	UPDATE [Nop_CustomerRole]
	SET
		[Name]=@Name,
		FreeShipping=@FreeShipping,
		TaxExempt=@TaxExempt,
		Active=@Active,
		Deleted=@Deleted

	WHERE
		CustomerRoleID = @CustomerRoleID
END
