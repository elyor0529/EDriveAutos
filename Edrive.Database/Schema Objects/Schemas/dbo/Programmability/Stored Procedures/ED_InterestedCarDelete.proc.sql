-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Delete City
-- =============================================
CREATE PROCEDURE [dbo].[ED_InterestedCarDelete]
(
	@InterestedCustomerID int
)
AS
BEGIN
	SET NOCOUNT ON
	Update [ED_InterestedCustomer]
	set
	IsActive = 0
	WHERE
		InterestedCustomerID = @InterestedCustomerID
END

