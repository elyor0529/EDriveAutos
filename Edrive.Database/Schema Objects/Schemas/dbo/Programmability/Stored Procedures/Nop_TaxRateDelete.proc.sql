﻿

CREATE PROCEDURE [dbo].[Nop_TaxRateDelete]
(
	@TaxRateID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_TaxRate]
	WHERE
		TaxRateID = @TaxRateID
END