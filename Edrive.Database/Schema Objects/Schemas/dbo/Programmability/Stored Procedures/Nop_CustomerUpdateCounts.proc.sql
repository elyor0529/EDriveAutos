

CREATE PROCEDURE [dbo].[Nop_CustomerUpdateCounts]
(
	@CustomerID int
)
AS
BEGIN

	DECLARE @NumPosts int

	SELECT 
		@NumPosts = COUNT(1)
	FROM
		[Nop_Forums_Post] fp
	WHERE
		fp.UserID = @CustomerID

	SET @NumPosts = isnull(@NumPosts, 0)
	
	SET NOCOUNT ON
	UPDATE 
		[Nop_Customer]
	SET
		[TotalForumPosts] = @NumPosts
	WHERE
		CustomerID = @CustomerID
END
