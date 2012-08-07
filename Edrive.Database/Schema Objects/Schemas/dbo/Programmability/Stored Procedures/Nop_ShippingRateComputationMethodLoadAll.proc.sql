

CREATE PROCEDURE [dbo].[Nop_ShippingRateComputationMethodLoadAll]
(	
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ShippingRateComputationMethod]
	WHERE @ShowHidden = 1 OR IsActive=1
	ORDER BY DisplayOrder
END
