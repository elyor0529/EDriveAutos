Update product set name= ( select Name from

(SELECT     dbo.Product_Make.Make +' '+ dbo.Product_Model.ModeLName +' '+ 
(case when  dbo.StateProvince.Name is null then ' ' else StateProvince.Name    end) 
+' '+  (case when  dbo.Customer.ZipPostalCode is null then  ' ' else convert(varchar,Customer.ZipPostalCode) end) 
+' '+Product.vin as Name,Product.Productid
FROM         dbo.Customer INNER JOIN
                      dbo.Product ON dbo.Customer.CustomerID = dbo.Product.CustomerID INNER JOIN
                      dbo.Product_Model ON dbo.Product.Model = dbo.Product_Model.id INNER JOIN
                      dbo.Product_Make ON dbo.Product_Model.MakeID = dbo.Product_Make.id AND dbo.Product_Model.MakeID = dbo.Product_Make.id Left Outer JOIN
                      dbo.StateProvince ON dbo.Customer.Stateid = dbo.StateProvince.StateProvinceID
                     
                    ) as tbl2 where tbl2.Productid=tbl1.Productid)
                    from product as tbl1