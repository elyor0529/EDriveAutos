-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Get Edrive Product by id
-- =============================================

CREATE PROCEDURE [dbo].[ED_EdriveManagementLoadByPrimaryKey]
(
	@ManagementId int
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT *		
	FROM [ED_EManagement]
	WHERE
		(ManagementId = @ManagementId)
	and deleted = 0
	
	SET @Err = @@Error

	RETURN @Err
END