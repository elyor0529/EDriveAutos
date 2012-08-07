-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Insert Edrive Products
-- =============================================

Create PROCEDURE [dbo].[ED_EGearInsert]
(
	@eGearID int = NULL output,
	@ProductName nvarchar(255),
	@Price money,
	@Qty int,
	@ImageID int,
	@ShortDesc nvarchar(Max),
	@DisplayOrder int,
	@Published bit,
	@Deleted bit,
	@CreatedOn datetime,
	@UpdatedOn datetime = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	INSERT
	INTO [ED_EGear] 
	(
		ProductName,
		Price ,
		Qty ,
		ImageID,
		ShortDesc,
		DisplayOrder,
		Published,
		Deleted,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@ProductName ,
		@Price,
		@Qty ,
		@ImageID,
		@ShortDesc,
		@DisplayOrder,
		@Published,
		@Deleted,
		@CreatedOn,
		@UpdatedOn
	)

	SET @Err = @@Error

	SELECT @eGearID = SCOPE_IDENTITY()

	RETURN @Err
END