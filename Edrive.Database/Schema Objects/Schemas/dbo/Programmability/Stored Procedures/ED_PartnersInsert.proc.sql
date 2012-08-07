-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <28/3/2011>
-- Description:	Insert Partners
-- =============================================

CREATE PROCEDURE [dbo].[ED_PartnersInsert]
(
	@PartnerId int = NULL output,
	@FirstName nvarchar(100),
	@LastName nvarchar(100),
	@Phone nvarchar(20),
	@Email nvarchar(100),
	@Company nvarchar(100),
	@Website nvarchar(100),
	@Comments varchar(MAX),
	@PictureId int,
	@IsApproved bit,
	@IsActive bit,
	@CreatedOn datetime,
	@UpdatedOn datetime = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	INSERT
	INTO [ED_Partners]
	(
		FirstName,
		LastName,
		Phone,
		Email,
		Company,
		Website,
		Comments,
		PictureId,
		IsApproved,
		IsActive,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@FirstName,
		@LastName,
		@Phone,
		@Email,
		@Company,
		@Website,
		@Comments,
		@PictureId,
		@IsApproved,
		@IsActive,
		@CreatedOn,
		@UpdatedOn
	)

	SET @Err = @@Error

	SELECT @PartnerId = SCOPE_IDENTITY()

	RETURN @Err
END



