

CREATE PROCEDURE [dbo].[Nop_NewsUpdate]
(
	@NewsID int,
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
	UPDATE [Nop_News]
	SET
		LanguageID=@LanguageID,
		Title=@Title,
		Short=@Short,
		[Full]=@Full,
		Published=@Published,
		AllowComments=@AllowComments,
		CreatedOn=@CreatedOn
	WHERE
		NewsID= @NewsID
END
