

CREATE PROCEDURE [dbo].[Nop_ProductTagLoadAll]
(
	@ProductID int,
	@Name nvarchar(100)
)
AS
BEGIN	

	SET @Name = isnull(@Name, '')
	
	SELECT pt1.*
	FROM [Nop_ProductTag] pt1
	WHERE pt1.ProductTagID IN
	(
		SELECT DISTINCT pt2.ProductTagID
		FROM [Nop_ProductTag] pt2
		LEFT OUTER JOIN [Nop_ProductTag_Product_Mapping] ptpm ON pt2.ProductTagID=ptpm.ProductTagID
		WHERE 
			(
				@ProductID IS NULL OR @ProductID=0
				OR ptpm.ProductID=@ProductID
			)
			AND
			(
				@Name = '' OR pt2.Name=@Name
			)
	)
	ORDER BY pt1.ProductCount DESC
END
