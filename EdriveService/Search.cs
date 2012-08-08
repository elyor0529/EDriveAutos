using System;
using System.Collections.Generic;
using System.Linq;
using EdriveService.DataContract;

namespace EdriveService
{
    public class Search : ISearch
    {
        EdriveService.edriveautoEntities _edriveEntity = new edriveautoEntities();
        /// <summary>
        /// Search on home page
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="CarsCount"></param>
        /// <param name="SortByColumn"></param>
        /// <returns></returns>
        public List<Products> SearchProductBy_Make_Model_City_Zip(string searchKey, int pageSize, int pageIndex, ref int? CarsCount, string SortByColumn,
            ref String Price, ref String Milage, ref String Make,
            ref string ModelID, ref String Year, ref String Body, ref String Type, Int32 Zip, String Warranty,
            ref String Vin, String Transmission, String Engine, ref String DriveType)
        {
            int saecrhString;
            bool serach_Key = Int32.TryParse(searchKey, out saecrhString);
            System.Linq.IQueryable<Product> Model = null ;
            if (SortByColumn != "Year" && SortByColumn != "Mileage" && SortByColumn != "Price")
                SortByColumn = "ProductId";
            if (SortByColumn == "Price")
                SortByColumn = "Price_Current";
            pageIndex = pageIndex == 0 ? 1 : pageIndex;// for 1st page Index
            var SkipRecords = (pageIndex - 1) * pageSize;
            List<EdriveService.DataContract.Products> lstModel = null;
            String expression = string.Empty;

            expression = BuildExpressionFor_SearchCars(ref Price, ref Milage, ref Make, ref  ModelID, ref Year, ref Body, ref Type, Zip,
            Warranty, ref Vin, Transmission, Engine, ref DriveType, ref pageSize, ref pageIndex);
            if (serach_Key == false)
                // Model = _edriveEntity.Product.Where(c => c.Product_Model.ModeLName.Contains(searchKey) || c.Product_Model.Product_Make.Make.Contains(searchKey)) ;//.OrderBy(c => c.ProductId).Skip(SkipRecords).Take(pageSize).ToList();
                Model = _edriveEntity.Product.Where(expression+" and "+
            "( it.Product_Model.ModeLName like '%" + searchKey + "%' or it.Product_Model.Product_Make.Make like '%" + searchKey + "%' ) ").OrderBy(c => c.ProductId).Skip(SkipRecords).Take(pageSize);
            else if(serach_Key==true)
                Model = _edriveEntity.Product.Where(expression+" and "+ " ( it.Zip = " + searchKey + ")").OrderBy(c => c.ProductId).Skip(SkipRecords).Take(pageSize);
            try
            {
            //    if (Convert.ToBoolean(expression) != true)
            //        Model = _edriveEntity.Product.Where("it.Product_Model.ModeLName like '%" + searchKey + "%' or it.Product_Model.Product_Make.Make like '%" + searchKey + "%' or '" + expression + "'");// || c.Product_Model.Product_Make.Make.Contains(searchKey)) ;//.OrderBy(c => c.ProductId).Skip(SkipRecords).Take(pageSize).ToList();
            //
            }
            catch
            { 
            }
          
            //var Model6 = Model.Where(expression).OrderBy(c => c.ProductId).Skip(SkipRecords).Take(pageSize).ToList();
            lstModel = Model.Select(m => new EdriveService.DataContract.Products
             {
                 productId = m.ProductId,
                 pics = m.Pics,
                 mileage = m.Mileage.Value,
                 body = m.Product_Body.Body,
                 price_Current = m.Price_Current.Value,
                 transmission = m.Transmission,
                 exterior = m.Exterior_Color,
                 OwnerDetail = m.OwnerDetail,
                 drive_Type = m.Drive_Type,
                 vin = m.VIN,
                 MakeName = m.Product_Model.Product_Make.Make,
                 ModelName = m.Product_Model.ModeLName,
                 zip = m.zip.Value,
                 Year = m.Year.Value,
                 model = m.Model,
                 updatedOn = m.UpdatedOn,
             }).ToList();
            if (CarsCount == null) CarsCount = _edriveEntity.Product.Where(expression).Count();
            return lstModel;
        }
        /// <summary>
        /// Build Expression for filter search
        /// </summary>
        /// <param name="Price"></param>
        /// <param name="Milage"></param>
        /// <param name="Make"></param>
        /// <param name="ModelID"></param>
        /// <param name="Year"></param>
        /// <param name="Body"></param>
        /// <param name="Type"></param>
        /// <param name="Zip"></param>
        /// <param name="Warranty"></param>
        /// <param name="Vin"></param>
        /// <param name="Transmission"></param>
        /// <param name="Engine"></param>
        /// <param name="DriveType"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        private String BuildExpressionFor_SearchCars(ref String Price, ref String Milage, ref String Make,
           ref string ModelID, ref String Year, ref String Body, ref String Type, Int32 Zip, String Warranty,
           ref String Vin, String Transmission, String Engine, ref String DriveType, ref Int32 pageSize, ref Int32 pageIndex)
        {
           
            if (ModelID != null)
            {
                if (ModelID.Contains(','))
                    ModelID = ModelID.Remove(ModelID.IndexOf(','));
            }
            pageIndex = pageIndex == 0 ? 1 : pageIndex + 1;//for first page condition
            pageIndex--;
            if (pageSize <= 0)
                pageSize = 25;
            var SkipRecords = pageIndex * pageSize;

            if (!string.IsNullOrEmpty(Make))
            {
                if (Make.IndexOf(',') > 0)
                    Make = Make.Substring(0, Make.LastIndexOf(','));
            }
            if (!string.IsNullOrEmpty(Year))
            {
                if (Year.IndexOf(',') > 0)
                    Year = Year.Substring(0, Year.LastIndexOf(','));
            }
            if (!string.IsNullOrEmpty(Body))
            {
                if (Body.IndexOf(',') > 0)
                    Body = Body.Substring(0, Body.LastIndexOf(','));
            }
            if (!string.IsNullOrEmpty(Type))
            {
                if (Type.IndexOf(',') > 0)
                    Type = Type.Substring(0, Type.LastIndexOf(','));
            }
            String expression = " true ";
            if (!String.IsNullOrEmpty(Price))
            {
                Price = Price.Substring(0, Price.LastIndexOf(','));
                String[] arPrice = Price.Split(',');
                var PriceExp = "";
                if (arPrice[0] != "-1")
                {
                    for (int i = 0; i < arPrice.Length; i++)
                    {
                        PriceExp = PriceExp + " ( it.Price_Current>=" + (Int32.Parse(arPrice[i].Split('-')[0])) + " and it.Price_Current<=" + (Int32.Parse(arPrice[i].Split('-')[1])) + " ) ";
                        if (i != arPrice.Length - 1)// not at last add or
                            PriceExp += " or ";
                    }
                }
                if (String.IsNullOrEmpty(PriceExp) == false)
                {
                    expression += String.Format(" and ( {0} ) ", PriceExp);
                }
            }
            if (!String.IsNullOrEmpty(Milage))
            {
                Milage = Milage.Substring(0, Milage.LastIndexOf(','));
                String[] arMilage = Milage.Split(',');
                var MileageExp = "";
                if (arMilage[0] != "-1")
                {
                    for (int i = 0; i < arMilage.Length; i++)
                    {
                        MileageExp = MileageExp + " ( it.Mileage>=" + (Int32.Parse(arMilage[i].Split('-')[0])) + " and it.Mileage<=" + (Int32.Parse(arMilage[i].Split('-')[1])) + " ) ";
                        if (i != arMilage.Length - 1)// not at last add or
                            MileageExp += " or ";
                    }
                }

                if (String.IsNullOrEmpty(MileageExp) == false)
                {
                    expression += String.Format(" and ( {0} ) ", MileageExp);
                }
            }
            //-----
            if (!string.IsNullOrEmpty(Make))
            {
                if (Convert.ToInt32(ModelID) <= 0)// Model is not set
                {
                    var MakeExp = "";
                    var arMake = Make.Split(',');
                    {
                        if (arMake[0] != "-1")
                        {
                            for (int i = 0; i < arMake.Length; i++)
                            {
                                var makeID = Convert.ToInt32(arMake[i]);
                                Int32[] arModelID = _edriveEntity.Product_Model.Where(m => m.MakeID == makeID).Select(m => m.id).ToArray();
                                for (int j = 0; j < arModelID.Length; j++)
                                {
                                    MakeExp += "  it.Model=" + arModelID[j].ToString() + " or ";
                                }
                            }
                        }
                    }
                    if (String.IsNullOrEmpty(MakeExp) == false)
                    {
                        MakeExp = MakeExp.Remove(MakeExp.LastIndexOf("or"));//this line is added only for model expression
                        expression += String.Format(" and ( {0} ) ", MakeExp);
                    }
                }
                else
                {
                    ModelID = ModelID.IndexOf(",") > 0 ? ModelID.Substring(0, ModelID.IndexOf(",")) : ModelID;
                    expression += String.Format(" and  it.Model={0}  ", ModelID);
                }
            }
            //-----------------
            String[] arYear=null;
            if (!string.IsNullOrEmpty(Year))
            {
                var YearExp = "";
                arYear = Year.Split(',');
                if (arYear[0] != "-1")
                {
                    for (int i = 0; i < arYear.Length; i++)
                    {
                        YearExp += " it.Year=" + Int32.Parse(arYear[i]);
                        if (i != arYear.Length - 1)// not at last add or
                            YearExp += " or ";
                    }
                }
                if (String.IsNullOrEmpty(YearExp) == false)
                {
                    expression += String.Format(" and ( {0} ) ", YearExp);
                }
            }
            //------------------
            if (!string.IsNullOrEmpty(Body))
            {
                var BodyExp = "";
                String[] arBody = Body.Split(',');
                if (arBody[0] != "-1")
                {
                    for (int i = 0; i < arBody.Length; i++)
                    {
                        BodyExp += " it.Body=" + Int32.Parse(arBody[i]);
                        if (i != arBody.Length - 1)// not at last add or
                            BodyExp += " or ";
                    }
                }
                if (String.IsNullOrEmpty(BodyExp) == false)
                {
                    expression += String.Format(" and ( {0} ) ", BodyExp);
                }
            }
            //-------------
            if (!string.IsNullOrEmpty(Type))
            {
                var TypeExp = "";
                String[] arType = Type.Split(',');
                if (arYear[0] != "-1")
                {
                    for (int i = 0; i < arType.Length; i++)
                    {
                        TypeExp += " it.Type=" + Int32.Parse(arType[i]);
                        if (i != arType.Length - 1)// not at last add or
                            TypeExp += " or ";
                    }
                }
                if (String.IsNullOrEmpty(TypeExp) == false)
                {
                    expression += String.Format(" and ( {0} ) ", TypeExp);
                }
                if (Zip >= 0)// zip is specified
                {
                    expression += " and " + "it.Zip=" + Zip;
                }
                if (Warranty == "0" || Warranty == "1")
                    expression += String.Format(" and Warranty={0} ", Warranty);
                if (Vin != "-1" && !String.IsNullOrEmpty(Vin))
                {
                    expression += String.Format(" and ( it.VIN='{0}' ) ", Vin);
                }
                if (DriveType != "-1" && !String.IsNullOrEmpty(DriveType))
                {
                    expression += String.Format(" and ( it.Vin='{0}' ) ", DriveType);
                }
                if (!String.IsNullOrEmpty(Transmission) && Transmission != "-1")
                    expression += String.Format(" and ( it.Transmission='{0}' ) ", Transmission);
                if (!String.IsNullOrEmpty(Engine) && Engine != "-1")
                    expression += String.Format(" and ( it.Engine='{0}' ) ", Engine);
                return expression;
            }
            return expression;
        }
    }

}