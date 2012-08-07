-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Update Edrive Products
-- =============================================

Create PROCEDURE [dbo].[ED_EGearUpdate]
(
	@eGearID int,
	@ProductName nvarchar(250),
	@Price money,
	@Qty int,
	@ShortDesc nvarchar(Max),
	@ImageID int,
	@UpdatedOn datetime,
	@DisplayOrder int,
	@Published bit
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [ED_EGear]
	SET
		ProductName =@ProductName,
		Price =@Price,
		Qty =@Qty,
		ShortDesc=@ShortDesc,
		ImageID=@ImageID,
		UpdatedOn=@UpdatedOn,
		DisplayOrder=@DisplayOrder,
		Published = @Published
	WHERE
		eGearID = @eGearID

	SET @Err = @@Error
	RETURN @Err
END