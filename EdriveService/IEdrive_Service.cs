using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using EdriveService.DataContract;

namespace EdriveService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface IEdrive_Service
	{
		#region "RajMethods"
		//--------Methods being used by window service
		#region "WindowServieMethods"
		[OperationContract]
		void UploadOnCarfax();
		[OperationContract]
		void DownloadFromCarfax();
		[OperationContract]
		string Data_Import_Update(String csvFilePath);

		/// <summary>
		/// Update DataBase with new records
		/// </summary>
		[OperationContract]
		string GetDataFromAutoBase();
		/// <summary>
		/// Update DataBase with new records
		/// </summary>
		[OperationContract]
		string GetDataFromGetAuto();
		/// <summary>
		/// Update DataBase with new records
		/// </summary>
		[OperationContract]
		string GetDataFromSchumacher();

		/// <summary>
		/// Update DataBase with new records
		/// </summary>
		[OperationContract]
		string GetDataFromAULtec();
		#endregion

		//--------Methods being used for Searching Cars info       
		#region "SearchMethods"



		[OperationContract]
		Int32 SearchCars_Count(String Price, String Milage, String Make, String ModelID, String Year,
		   String Body, String Type, Int32 Zip, String Warranty, String Vin, String Transmission, String Engine, String DriveType, String sortByColumn, Int32 pageSize, Int32 pageIndex)
		;
		/// <summary>
		/// Method used for filter searching on search page
		/// </summary>
		/// <param name="Price"></param>
		/// <param name="Milage"></param>
		/// <param name="Make"></param>
		/// <param name="Year"></param>
		/// <param name="Type"></param>
		/// <param name="Zip"></param>
		/// <param name="sortByColumn"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <param name="CarsCount">if null it updates the Cars Count else not update the Cars Count</param>
		/// <returns></returns>
		[OperationContract]
		List<Products> SearchCars(String Price, String Milage, String Make, String ModelID,
		String Year, String Body, String Type, Int32 Zip, String Warranty, String Vin, String Transmission, String Engine, String DriveType, String sortByColumn,
		Int32 pageSize, Int32 pageIndex, ref Int32? CarsCount);
		/// <summary>
		/// Method returns cars search result for home page search operation
		/// </summary>
		/// <param name="makeID"></param>
		/// <param name="modelId"></param>
		/// <param name="zipCode"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <param name="CarsCount">if null it updates the Cars Count else not update the Cars Count</param>
		/// <returns></returns>
		[OperationContract]
		List<DataContract.Products> searchProduct(int makeID, int modelId, String zipCode, Int32 pageSize, Int32 pageIndex, ref Int32? CarsCount, String SortByColumn);

		[OperationContract]
		void Qualify_All_Products();


		[OperationContract]
		void Qualify_All_Products_to_RecoverDeletedProducts();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="VIN"></param>
		/// <param name="SellerEmail"></param>
		/// <param name="SellerName"></param>
		/// <param name="SellerNotes"></param>
		/// <param name="Phone"></param>
		/// <param name="status"></param>
		/// <param name="Msg"></param>
		/// <param name="OptionsID"></param>
		/// <returns></returns>

		[OperationContract]
		Boolean AddProductUsingNadaService(String VIN, String Condition, Int32 mileage, Int32 SellerZip, String SellerEmail, String SellerName, String SellerNotes, String Phone, Boolean Offer, ref String Msg)
	;
		/// <summary>
		/// Add New Product n return true on success
		/// </summary>
		/// <param name="Prd"></param>
		/// <returns></returns>
		[OperationContract]
		Products AddProduct(Products Prd, out Boolean status, ref String Msg, String[] OptionsID);


		/// <summary>
		/// To return the proudct rating by its product id
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		[OperationContract]
		Int32 GetProductRating(Int32 productId);
		/// <summary>
		/// to add the product rating for product Details page
		/// </summary>
		/// <param name="id"></param>
		/// <param name="score"></param>
		/// <param name="p"></param>
		/// <param name="Msg"></param>
		[OperationContract]
		Boolean AddProductRating(int id, int score, string p, out string Msg);

		[OperationContract]
		void AddProductPicture(int ProductId, string fileName);
		/// <summary>
		/// Method returns total cars count search result for AdvanceSearch operation
		/// </summary>
		/// <param name="_attributes"></param>
		/// <returns>List of Cars</returns>
		[OperationContract]
		Int32 GetAdvancedSearchProducts_Count(AdvancedSearchAttributes _attributes);
		/// <summary>
		/// Method returns total cars list search result for AdvanceSearch operation
		/// </summary>
		/// <param name="_attributes"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <returns>List of Cars</returns>
		[OperationContract]
		List<EdriveService.DataContract.Products> GetAdvancedSearchProducts(AdvancedSearchAttributes _attributes, int pageSize, int pageIndex, String SortByColumn);
		#endregion

		#endregion


		#region "Harpreet Methods"
		/// <summary>
		/// To get Distinct Driver type of all product
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		List<String> BindDriveType();
		//Created on 04-01-2012,Created By harpreet Singh
		//Home Search Methods
		//[OperationContract]
		//int searchProduct_Count(int modelId, string zipCode);
		[OperationContract]
		List<EdriveService.DataContract.Product_Model> BindModel(int makeid);
		[OperationContract]
		List<EdriveService.DataContract.Product_Make> BindMake();
		//[OperationContract]
		//List<EdriveService.DataContract.Products> searchProduct(int modelId, string zipCode);
		[OperationContract]
		//Created on 04-01-2012,Created By harpreet Singh
		//Advanced Search Methods
		List<EdriveService.DataContract.Product_Make> bindMakeAttributes();
		[OperationContract]
		List<EdriveService.DataContract.Product_Type> bindtypeAttributes();
		[OperationContract]
		List<EdriveService.DataContract.Product_Body> BindBodyType();
		[OperationContract]
		List<EdriveService.DataContract.Products> GetTransmission();
		[OperationContract]
		List<EdriveService.DataContract.Products> GetEngine();
		/// <summary>
		/// select products based upon year and price
		/// </summary>
		/// <returns></returns>
		//[OperationContract]
		//List<EdriveService.DataContract.Products> GetAdvancedSearchProducts(AdvancedSearchAttributes _attributes);
		#endregion

		#region "Products Method"
		/// <summary>
		/// return the product by its product id
		/// </summary>
		/// <param name="ProductID"></param>
		/// <returns></returns>
		[OperationContract]
		DataContract.Products GetProductByID(int ProductID);

		/// <summary>
		/// To return the list of multiple productsds
		/// </summary>
		/// <param name="productIDs"></param>
		/// <returns></returns>
		[OperationContract]
		List<DataContract.Products> GetProductByIDs(List<int> productIDs);
		/// <summary>
		/// It retunr the list of Feature vehicle specifed in the count
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		List<Products> Get_FeaturedVehicles(Int32 VehCounts);
		/// <summary>
		/// Check whether product exist by its uniques VIN
		/// </summary>
		/// <param name="Vin"></param>
		/// <returns></returns>
		[OperationContract]
		Boolean IsProductExist_by_VIN(String Vin);
		/// <summary>
		/// if product exist for the same vin for other proeduct
		/// </summary>
		/// <param name="Vin"></param>
		/// <returns></returns>
		[OperationContract]
		Boolean IsProductExist_by_VIN_for_other_vehicle(String Vin, Int32 ProductID);


		[OperationContract]
		List<DataContract.Products> GetProductsByDealerID(Int32 DealerID);
		[OperationContract]
		void AddProductOptions(string OPTIONS, Int32 ProductID);
		//[OperationContract]
		//String[] GetProductOptions(Int32 ProductID);
		[OperationContract]
		List<EdriveService.DataContract.ProductOptions> GetAllProductOptions();
		[OperationContract]
		List<EdriveService.DataContract.ProductOptions> GetAllProductOptions_By_ProductID(Int32 ProductID);
		#endregion


		[OperationContract]
		void AddPicsToProduct(String[] Pics, Int32 ProductID);
		[OperationContract]
		DataContract.Product_Make GetMakeById(Int32 makeId);
		[OperationContract]
		DataContract.Product_Body GetBodybyId(Int32 bodyid);
		[OperationContract]
		DataContract.Product_Model GetModelbyId(Int32 ModelID);
		[OperationContract]
		DataContract.Customer GetDealerbyProductID(Int32 ProductID);

		#region "ManageProduct"
		[OperationContract]
		List<Products> SearchProduct_for_ManageProduct(string VIN, string Stock, Int32 makeId, String _ComapnyName, String _DealerEmail, String RoleName, bool? isOnlyFeatured, Int32 pageIndex, Int32 pageSize, ref Int32 CarsCount);


		/// <summary>
		/// return the list of All Dealers
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		List<DataContract.Customer> GetDealers();

		/// <summary>
		/// This method return the list of dealer whose producta have updated in last 45 days
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		List<DataContract.Customer> GetDealers_for_ManageProoduct(Int32 CountDownDays);
		/// <summary>
		/// To get The list Company Name
		/// </summary>
		/// <returns></returns>
		//[OperationContract]
		//List<String> GetDealerCompanyName();
		/// <summary>
		/// Delete Products from DB
		/// </summary>
		/// <param name="ProductIDs">Products to be Deleted</param>
		[OperationContract]
		Boolean DeleteProduct(string[] ProductIDs);
		/// <summary>
		/// To return the no of total records for manage product section
		/// </summary>
		/// <param name="Vin"></param>
		/// <param name="Stock"></param>
		/// <param name="_MakeID"></param>
		/// <param name="_DealerID"></param>
		/// <param name="Featured"></param>
		[OperationContract]
		Int32 Get_Count_SearchProduct_for_ManageProduct
	   (string VIN, string Stock, Int32 makeId, String _ComapnyName, String _DealerEmail, String RoleName, bool? isOnlyFeatured);

		[OperationContract]
		Boolean ApproveProducts();

		[OperationContract]
		bool QualifyPriceofProduct(List<string> ProductIDs);
		[OperationContract]
		List<Product_Picture> GetProductPicture_By_ProductID(Int32 ProductID);
		[OperationContract]

		Products UpdateProduct(Products Prd, out bool status, ref string Msg, string[] OptionsID);
		[OperationContract]
		Boolean Update_Product_Picture(ref List<Product_Picture> Pictures, Int32 ProductID, out String Msg);
		//[OperationContract]
		//Boolean DeleteProduct(out String Msg, Int32 ProductId);

		#endregion

		#region "Manage Dealer"
		/// <summary>
		/// It return the List of Country
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		List<DataContract.Country> GetCountry();
		/// <summary>
		/// It return the List of State by its CountryID
		/// </summary>
		/// <param name="CountryID"></param>
		/// <returns></returns>
		[OperationContract]
		List<DataContract.State> GetStateByCountry(Int32 CountryID);

		/// <summary>
		/// It State Details by its Stateid
		/// </summary>
		/// <param name="CountryID"></param>
		/// <returns></returns>
		[OperationContract]

		DataContract.State GetState_Detail_By_StateID(Int32 StateID);
		/// <summary>
		/// It makes the specified the Dealer as featured dealer.
		/// </summary>
		/// <param name="DealerId"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		[OperationContract]
		Boolean MakeDealer_as_FeaturedDealer(int DealerId, out String Msg);
		/// <summary>
		/// It return the List of vehicle of featured dealers
		/// </summary>
		/// <param name="rec_to_return"></param>
		/// <returns></returns>
		[OperationContract]
		List<Products> Get_Dealers_FeaturedVehicle(Int32 rec_to_return);
		/// <summary>
		/// It Creates new Dealer and retunr true on Succes and update Msg if it falis
		/// </summary>
		/// <param name="cust"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		[OperationContract]
		Boolean AddDealer(DataContract.Customer cust, ref String Msg, DataContract._CustomerProfile Profile)
	   ;


		[OperationContract]
		Boolean UpdatePassword_for_Dealer(Int32 _DealerId, String Pwd, out String Msg);

		/// <summary>
		/// it   Update Dealer and return true on Succes and update Msg if it falis
		/// </summary>
		/// <param name="cust"></param>
		/// <param name="Msg"></param>
		/// <param name="Profile"></param>
		/// <returns></returns>
		[OperationContract]
		Boolean Update_Dealer(DataContract.Customer cust, ref String Msg, DataContract._CustomerProfile Profile);

		/// <summary>
		/// Gets a list of dealers by zipcode
		/// </summary>
		/// <param name="zipcode"></param>
		/// <returns></returns>
		[OperationContract]
		List<DataContract.Customer> GetDealersByZip(string zipcode);

		/// <summary>
		/// it   Update Dealer and return true on Succes and update Msg if it falis
		/// </summary>
		/// <param name="cust"></param>
		/// <param name="Msg"></param>
		/// <param name="Profile"></param>
		/// <returns></returns>
		[OperationContract]
		Boolean UpdateAdmin_InfoDetails(DataContract.Customer cust, ref String Msg);
		/// <summary>
		/// Update the Product's picture listing in table and also update Pictures with updated ProductpictureID,return true on success and update Msg withe exception
		/// </summary>
		/// <param name="Pictures"></param>
		/// <param name="ProductID"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		[OperationContract]
		Boolean Update_Dealer_Personal_Details(out String Msg, DataContract.Customer _customer);

		[OperationContract]
		DataContract.ED_Zipcodes GetED_ZipCodes(String ZipCode);
		#endregion
		[OperationContract]
		List<DataContract.Products> GetHotSheet(double? latitude, double? longitude,
	   double NewLatitude, double NewLongitude, int makeID, string Zipcode, Int32 pageIndex, Int32 pageSize, out Int32 CarsCount)
 ;
		/// <summary>
		/// This return if users exist for the specified Username,and Password and its Rolename n on success update the Customer details
		/// </summary>
		/// <param name="UserName"></param>
		/// <param name="Password"></param>
		/// <param name="RoleName"></param>
		/// <param name="customer"></param>
		/// <returns></returns>
		[OperationContract]
		Boolean Authenticate_Dealer_or_Admin(String UserName, String Password, String RoleName, ref DataContract.Customer customer);
		/// <summary>
		/// This return the Dealers Details by its Email(Username)
		/// </summary>
		/// <param name="Email"></param>
		/// <returns></returns>
		[OperationContract]
		DataContract.Customer GetDealerByDealerEmail(String Email);
		/// <summary>
		/// This return the Dealers Details by its  CustomerID
		/// </summary>
		/// <param name="CustomerID"></param>
		/// <returns></returns>
		[OperationContract]
		DataContract.Customer GetDealerByDealerID(int? CustomerID);
		/// <summary>
		/// To get the Dealer Profile by its DealerID
		/// </summary>
		/// <param name="CustomerID"></param>
		/// <returns></returns>
		[OperationContract]
		DataContract._CustomerProfile GetDealer_Profile_ByDealerID(int? CustomerID);

		/// <summary>
		/// Gets the List of States by CountryID
		/// </summary>
		/// <param name="CountryID"></param>
		/// <returns></returns>
		List<_StateProvince> GetStateProvince_By_CountryID(Int32 CountryID);
		/// <summary>
		/// This returns the couns of Seller and Dealers for Hot Sheet page.
		/// </summary>
		/// <param name="zip"></param>
		/// <param name="Make"></param>
		/// <param name="lat1"></param>
		/// <param name="lat2"></param>
		/// <param name="long1"></param>
		/// <param name="long2"></param>
		/// <param name="SellerCount"></param>
		/// <param name="DealerCount"></param>  

		[OperationContract]
		DataContract._SellerCount GetSellerCount_for_HotSheet(String zip, Int32? Make, double? lat1, double? lat2, double? long1, double? long2);
		[OperationContract]
		List<DataContract.Customer> SearchDealer_For_ManagDealerSection(string CompanyName, string CompanyName2,
		string Email, string LastName, string Name, DateTime? RegFrom, DateTime? RegTo, int pageIndex,
		int pageSize, out int CarsCount);
		[OperationContract]
		Boolean IsDealerExists(String Email);
		/// <summary>
		/// If other Dealer exists for the same email
		/// </summary>
		/// <param name="Email"></param>
		/// <returns></returns>
		[OperationContract]
		Boolean Is_other_DealerExist_for_same_email(String Email, Int32 currrent_CustomerId);

		[OperationContract]
		Boolean IntrestedCustomer(DataContract.IntrestedCustomer _customer, int _productid);
		[OperationContract]
		Boolean Delete_Dealer(Int32 CustomerID);
		/// <summary>
		/// To delet the Dealer profile page image

		/// </summary>
		/// <param name="CustomerID"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		[OperationContract]
		Boolean Delete_Dealer_Profile_PageImage(int CustomerID, out string Msg)
			;
		/// <summary>
		/// To delet the Dealer profile logo image
		/// </summary>
		/// <param name="CustomerID"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
		[OperationContract]
		Boolean Delete_Dealer_Profile_Logo(int CustomerID, out string Msg);


		[OperationContract]
		List<DataContract.Products> SearchProductBy_Make_Model_City_Zip(String searchKey, Int32 pageSize, Int32 pageIndex, ref Int32? CarsCount, String SortByColumn, ref String Price, ref String Milage, ref String Make,
			ref string ModelID, ref String Year, ref String Body, ref String Type, Int32 Zip, int? radius, String Warranty,
			ref String Vin, String Transmission, String Engine, ref String DriveType,
			Boolean isSearchByDealer, Int32 SearchByDealerID);


		/// <summary>
		/// to get the total CarsCounts fo search expression
		/// </summary>
		/// <param name="searchKey"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <param name="CarsCount"></param>
		/// <param name="SortByColumn"></param>
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
		/// <param name="isSearchByDealer"></param>
		/// <param name="SearchByDealerID"></param>

		[OperationContract]
		void SearchProductBy_Make_Model_City_Zip_Count(string searchKey, int pageSize, int pageIndex, ref int? CarsCount, string SortByColumn,
			ref String Price, ref String Milage, ref String Make,
			ref string ModelID, ref String Year, ref String Body, ref String Type, Int32 Zip, int? radius, String Warranty,
			ref String Vin, String Transmission, String Engine, ref String DriveType, Boolean isSearchByDealer, Int32 SearchByDealerID);

		/// <summary>
		/// To return the total Cars Count on header of page.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		Int32 Get_TotalVehicles_Count();
		/// <summary>
		/// To return the total Dealers Count on header of page.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		Int32 Get_TotalDealers_Count();

		/// <summary>
		/// To return the list of interested customer
		/// </summary>
		/// <param name="InterestType"></param>
		/// <returns></returns>
		[OperationContract]
		List<DataContract.IntrestedCustomer> Get_InterestedCustomer(Int32 InterestType);

		[OperationContract]
		Boolean Delete_InterestedCustomer(int id, out  String Msg);

		[OperationContract]
		List<Products> SearchCars_for_home_Page(String searchKey, Int32 Counts_to_ret);
		[OperationContract]
		string GetDealerGUID(Int32 customerid);
	}




	[DataContract]
	public class CompositeType
	{
		bool boolValue = true;
		string stringValue = "Hello ";

		[DataMember]
		public bool BoolValue
		{
			get { return boolValue; }
			set { boolValue = value; }
		}

		[DataMember]
		public string StringValue
		{
			get { return stringValue; }
			set { stringValue = value; }
		}
	}

}
