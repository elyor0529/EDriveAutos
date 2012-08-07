using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.Models
{
    public class SearchFilter
    {
        public Int32 Make { get; set; }
        public Int32 Body { get; set; }
        public string Engine { get; set; }
        [Range(0, 2147483647)]
        [StringLength(9, ErrorMessage = "Mileage length can be maximum of 9 digit.")]
        public Int32? Mileage { get; set; }
        public string Transmission { get; set; }
        //[RegularExpression(@"^(\s*|\d+)$",ErrorMessage="only numbers allowed in zip")]

        //   [RegularExpression(@"^(\s*|\d+)$")]
        public Int32? Zip { get; set; }
        public string Vin { get; set; }
        public Int32 Radius { get; set; }
        public Int32 Model { get; set; }
        [DisplayName("Year From")]
        [Minimun(MinValue=1998,ErrorMessage="Minimun Year should be greater than 1997")]
        //[Range(1998, 2012, ErrorMessage = "Range Must be between 1998-2012")]
        public int? YearMin { get; set; }
        //[Range(1998, 2012, ErrorMessage = "Range Must be between 1998-2012")]
        [DisplayName("Year To")]
        [Minimun(MinValue = 1998, ErrorMessage = "Maximum Year should be greater than 1997")]
        public int? YearMax { get; set; }
        [DisplayName("Min Price")]
        public int? PriceMin { get; set; }
        [DisplayName("Max Price")]
        public int? PriceMax { get; set; }
        public String DriveType { get; set; }
        public Int32 Type { get; set; }
        public string Warranty { get; set; }
    }

	public class NoResultsSearchFilter
	{
		public Int32 NoResultsMake { get; set; }
		public Int32 NoResultsBody { get; set; }
		public string NoResultsEngine { get; set; }
		[Range(0, 2147483647, ErrorMessage = "Mileage length can be maximum of 9 digits.")]
		public Int32? NoResultsMileage { get; set; }
		public string NoResultsTransmission { get; set; }
		//[RegularExpression(@"^(\s*|\d+)$",ErrorMessage="only numbers allowed in zip")]

		//   [RegularExpression(@"^(\s*|\d+)$")]
		[Required(ErrorMessage = "Please enter a zipcode")]
		[Range(0, 99999, ErrorMessage = "Please enter a valid zipcode")]
		public Int32? NoResultsZip { get; set; }
		public string NoResultsVin { get; set; }
		public Int32 NoResultsRadius { get; set; }

		public Int32 NoResultsModel { get; set; }
		[DisplayName("Year From")]
		[Minimun(MinValue = 1998, ErrorMessage = "Minimun Year should be greater than 1997")]
		//[Range(1998, 2012, ErrorMessage = "Range Must be between 1998-2012")]
		public int? NoResultsYearMin { get; set; }
		//[Range(1998, 2012, ErrorMessage = "Range Must be between 1998-2012")]
		[DisplayName("Year To")]
		[Minimun(MinValue = 1998, ErrorMessage = "Maximum Year should be greater than 1997")]
		public int? NoResultsYearMax { get; set; }
		[DisplayName("Min Price")]
		public int? NoResultsPriceMin { get; set; }
		[DisplayName("Max Price")]
		public int? NoResultsPriceMax { get; set; }
		public String NoResultsDriveType { get; set; }
		public Int32 NoResultsType { get; set; }
		public string NoResultsWarranty { get; set; }
	}

    public class MinimunAttribute : ValidationAttribute
    {
        public double MinValue { get; set; }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            var price = Convert.ToDouble(value);
            if (price < MinValue)
            {
                return false;
            }
            return true;
        }
    }

    public class MinimunValidator : DataAnnotationsModelValidator<MinimunAttribute>
    {
        double _minValues;
        string _message;
        public MinimunValidator(ModelMetadata metadata, ControllerContext context
          , MinimunAttribute attribute)
            : base(metadata, context, attribute)
        {
            _minValues = attribute.MinValue;
            _message = attribute.ErrorMessage;
        }

        public override IEnumerable<ModelClientValidationRule>
         GetClientValidationRules()
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = _message,
                ValidationType = "Mininmin Values"
            };
            rule.ValidationParameters.Add("min", _minValues);
            return new[] { rule };
        }
    }
}