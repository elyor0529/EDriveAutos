

CREATE PROCEDURE [dbo].[Nop_ProductTypeLoadByPrimaryKey]
(
	@ProductTypeID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductType]
	WHERE
		(ProductTypeID = @ProductTypeID)
END
