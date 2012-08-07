

CREATE PROCEDURE [dbo].[Nop_LogInsert]
(
	@LogID int = NULL output,
	@LogTypeID int,
	@Severity int,
	@Message nvarchar(1000),
	@Exception nvarchar(4000),
	@IPAddress nvarchar(100),
	@CustomerID int,
	@PageURL nvarchar(100),
	@ReferrerURL nvarchar(100),
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Log]
	(
		LogTypeID,
		Severity,
		[Message],
		Exception,
		IPAddress,
		CustomerID,
		PageURL,
		ReferrerURL,
		CreatedOn
	)
	VALUES
	(
		@LogTypeID,
		@Severity,
		@Message,
		@Exception,
		@IPAddress,
		@CustomerID,
		@PageURL,
		@ReferrerURL,
		@CreatedOn
	)

	set @LogID=SCOPE_IDENTITY()
END
