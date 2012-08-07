

CREATE PROCEDURE [dbo].[Nop_StateProvinceDelete]
(
	@StateProvinceID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_StateProvince]
	WHERE
		StateProvinceID = @StateProvinceID

	DELETE 
	FROM Nop_TaxRate
	WHERE
		StateProvinceID = @StateProvinceID
END
