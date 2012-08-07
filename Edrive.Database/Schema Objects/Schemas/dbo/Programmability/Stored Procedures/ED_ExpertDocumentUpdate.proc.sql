
-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Update Expert Document
-- =============================================

CREATE PROCEDURE [dbo].[ED_ExpertDocumentUpdate]
(
	@EDId int,
	@Title nvarchar(150),
	@Description varchar(MAX),
	@DocumentPath nvarchar(150),
	@UpdatedOn datetime = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [ED_ExpertDocuments]
	SET		
		[Title] = @Title,
		[Description] = @Description,
		[DocumentPath] = @DocumentPath,
		[UpdatedOn] = @UpdatedOn
	WHERE
		[EDId] = @EDId


	SET @Err = @@Error


	RETURN @Err
END



