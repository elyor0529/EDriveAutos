

CREATE PROCEDURE [dbo].[Nop_NewsInsert]
(
	@NewsID int = NULL output,
	@LanguageID	int,
	@Title nvarchar(1000),
	@Short nvarchar(4000),
	@Full nvarchar (max),
	@Published bit,
	@AllowComments bit,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_News]
	(
		LanguageID,
		Title,
		Short,
		[Full],
		Published,
		AllowComments,
		CreatedOn
	)
	VALUES
	(
		@LanguageID,
		@Title,
		@Short,
		@Full,
		@Published,
		@AllowComments,
		@CreatedOn
	)

	set @NewsID=SCOPE_IDENTITY()
END
