-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Get Dealers FAQ by id
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetTopCarfax]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT top(1) *
	FROM ED_CarfaxLogDetail
	order by CarFax_logID desc
		
	SET @Err = @@Error

	RETURN @Err
END