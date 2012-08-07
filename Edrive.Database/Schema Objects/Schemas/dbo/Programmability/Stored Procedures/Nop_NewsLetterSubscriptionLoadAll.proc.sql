

CREATE PROCEDURE [dbo].[Nop_NewsLetterSubscriptionLoadAll]
(
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		nls.* 
	FROM
		[Nop_NewsLetterSubscription] nls
	LEFT OUTER JOIN 
		Nop_Customer c 
	ON 
		nls.Email=c.Email
	WHERE
		(nls.Active = 1 OR @ShowHidden = 1) AND 
		(c.CustomerID IS NULL OR (c.Active = 1 AND c.Deleted = 0))
END
