

CREATE PROCEDURE [dbo].[Nop_DiscountTypeLoadAll]

AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_DiscountType]
	ORDER BY DiscountTypeID
END
