﻿

CREATE PROCEDURE [dbo].[Nop_LogLoadAll]

AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Log]
	ORDER BY CreatedOn desc
END
