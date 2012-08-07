using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace EdriveService
{
    [ServiceContract]
    public interface ISearch
    {
        //Created by Harpreet singh on 14-02-2012
        /// <summary>
        /// Method for home search product by make,model,city and zip code only search key will be passed to the method
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="CarsCount"></param>
        /// <param name="SortByColumn"></param>
        /// <returns></returns>
        [OperationContract]
        List<DataContract.Products> SearchProductBy_Make_Model_City_Zip(String searchKey, Int32 pageSize, Int32 pageIndex, ref Int32? CarsCount, String SortByColumn, ref String Price, ref String Milage, ref String Make,
            ref string ModelID, ref String Year, ref String Body, ref String Type, Int32 Zip, String Warranty,
            ref String Vin, String Transmission, String Engine, ref String DriveType);
       
    }
}