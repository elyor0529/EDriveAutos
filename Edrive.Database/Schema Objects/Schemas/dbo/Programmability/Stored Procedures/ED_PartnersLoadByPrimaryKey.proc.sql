-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <28/3/2011>
-- Description:	Get Partner by id
-- =============================================

CREATE PROCEDURE [dbo].[ED_PartnersLoadByPrimaryKey]
(
	@PartnerId int
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT *		
	FROM [ED_Partners]
	WHERE
		([PartnerId] = @PartnerId)

	SET @Err = @@Error

	RETURN @Err
END


