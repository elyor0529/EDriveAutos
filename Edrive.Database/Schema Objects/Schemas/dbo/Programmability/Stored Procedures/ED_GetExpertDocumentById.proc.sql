-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Get FAQ by id
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetExpertDocumentById]
(
	@EDId bigint
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT * 
	FROM [ED_ExpertDocuments]
	WHERE
		([EDId] = @EDId)

	SET @Err = @@Error

	RETURN @Err
END


