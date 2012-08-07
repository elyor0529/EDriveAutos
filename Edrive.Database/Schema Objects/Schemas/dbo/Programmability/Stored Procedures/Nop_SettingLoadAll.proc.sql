

CREATE PROCEDURE [dbo].[Nop_SettingLoadAll]
	
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Setting]
	ORDER BY [name]
END
