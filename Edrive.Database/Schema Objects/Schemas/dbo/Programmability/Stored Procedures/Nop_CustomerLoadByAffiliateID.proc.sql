

CREATE PROCEDURE [dbo].[Nop_CustomerLoadByAffiliateID]
(
	@AffiliateID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Customer]
	WHERE 
		AffiliateID=@AffiliateID and Deleted=0
	order by RegistrationDate desc
END
