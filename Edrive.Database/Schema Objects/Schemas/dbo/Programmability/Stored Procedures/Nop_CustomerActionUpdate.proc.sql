

CREATE PROCEDURE [dbo].[Nop_CustomerActionUpdate]
(
	@CustomerActionID int,
	@Name nvarchar(100),
	@SystemKeyword nvarchar(100),
	@Comment nvarchar(1000),
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_CustomerAction]
	SET
		[Name]=@Name,
		[SystemKeyword]=@SystemKeyword,
		[Comment]=@Comment,
		DisplayOrder=@DisplayOrder
	WHERE
		[CustomerActionID] = @CustomerActionID
END
