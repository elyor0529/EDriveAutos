-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Update DealersFAQ
-- =============================================

Create PROCEDURE [dbo].[ED_UpdateProductState]
(
	@VIN nvarchar(MAX),
	@StateID int,
	@UpdatedOn datetime = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [Nop_Product] 
	SET		
		StateID =@StateID
	WHERE
		VIN =@VIN

	SET @Err = @@Error


	RETURN @Err
END