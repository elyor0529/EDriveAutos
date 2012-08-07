

CREATE PROCEDURE [dbo].[Nop_StateProvinceLoadByPrimaryKey]
(
	@StateProvinceID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_StateProvince]
	WHERE
		StateProvinceID = @StateProvinceID
END
