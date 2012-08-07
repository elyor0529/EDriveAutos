-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <3/2/2011>
-- Description:	Delete FAQ
-- =============================================
CREATE PROCEDURE [dbo].[ED_UpdatePoductZipcode] --'134'
	@CustomerId int,
	@Dealer_Zip varchar(250)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Nop_Product SET
	Dealer_Zip=@Dealer_Zip
	WHERE CustomerID=@CustomerID
END