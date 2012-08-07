

CREATE PROCEDURE [dbo].[Nop_SearchLogInsert]
(
	@SearchLogID int = NULL output,
	@SearchTerm nvarchar(100),
	@CustomerID int,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_SearchLog]
	(
		SearchTerm,
		CustomerID,
		CreatedOn
	)
	VALUES
	(
		@SearchTerm,
		@CustomerID,
		@CreatedOn
	)

	set @SearchLogID=SCOPE_IDENTITY()
END
