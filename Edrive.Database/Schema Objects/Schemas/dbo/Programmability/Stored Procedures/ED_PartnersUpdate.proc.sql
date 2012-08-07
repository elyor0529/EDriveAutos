-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <28/3/2011>
-- Description:	Update Partners
-- =============================================

CREATE PROCEDURE [dbo].[ED_PartnersUpdate]
(
	@PartnerId int,
	@FirstName nvarchar(100),
	@LastName nvarchar(100),
	@Phone nvarchar(20),
	@Email nvarchar(100),
	@Company nvarchar(100),
	@Website nvarchar(100),
	@Comments varchar(MAX),
	@PictureId int,
	@IsApproved bit,
	@UpdatedOn datetime
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [ED_Partners]
	SET
		FirstName=@FirstName,
		LastName=@LastName,
		Phone=@Phone,
		Email=@Email,
		Company=@Company,
		Website=@Website,
		Comments=@Comments,
		PictureId=@PictureId,
		IsApproved=@IsApproved,
		UpdatedOn=@UpdatedOn
	WHERE
		[PartnerId] = @PartnerId

	SET @Err = @@Error
	RETURN @Err
END





