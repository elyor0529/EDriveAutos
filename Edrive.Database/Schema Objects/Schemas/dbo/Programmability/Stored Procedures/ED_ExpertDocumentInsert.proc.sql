
-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Insert Expert Document
-- =============================================

CREATE PROCEDURE [dbo].[ED_ExpertDocumentInsert]
(
	@EDId int = NULL output,
	@Title nvarchar(150),
	@Description varchar(MAX),
	@DocumentPath nvarchar(150),
	@IsActive bit,
	@CreatedOn datetime,
	@UpdatedOn datetime = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	INSERT
	INTO [ED_ExpertDocuments]
	(
		[Title],
		[Description],
		[DocumentPath],
		[IsActive],
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES
	(
		@Title,
		@Description,
		@DocumentPath,
		@IsActive,
		@CreatedOn,
		@UpdatedOn
	)

	SET @Err = @@Error

	SELECT @EDId = SCOPE_IDENTITY()

	RETURN @Err
END


