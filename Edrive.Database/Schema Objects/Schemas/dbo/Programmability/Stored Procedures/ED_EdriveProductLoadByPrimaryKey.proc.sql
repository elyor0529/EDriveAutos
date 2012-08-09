﻿-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Get Edrive Product by id
-- =============================================

CREATE PROCEDURE [dbo].[ED_EdriveProductLoadByPrimaryKey]
(
	@EDProductId int
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT *		
	FROM [ED_EdriveProducts]
	WHERE
		([EDProductId] = @EDProductId)

	SET @Err = @@Error

	RETURN @Err
END


