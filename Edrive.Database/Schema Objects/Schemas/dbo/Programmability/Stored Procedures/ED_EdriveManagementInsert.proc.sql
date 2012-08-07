-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Insert Edrive Products
-- =============================================

CREATE PROCEDURE [dbo].[ED_EdriveManagementInsert]
(
	@ManagementId int = NULL output,
	@Title nvarchar(250),
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
	INTO [ED_EManagement]
	(
		Title,
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
		@Title,
		@ImageID,
		@ShortDesc,
		@DisplayOrder,
		@Published,
		@Deleted,
		@CreatedOn,
		@UpdatedOn
	)

	SET @Err = @@Error

	SELECT @ManagementId = SCOPE_IDENTITY()

	RETURN @Err
END