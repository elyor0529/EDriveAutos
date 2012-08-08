using CsvHelper.Configuration;

namespace EdriveService
{
    public class CsvRecord
    {
        [CsvField(Index = 0)]
        public string Type { get; set; }
        [CsvField(Index = 1)]
        public string Stock { get; set; }
        [CsvField(Index = 2)]
        public string Vin { get; set; }
        [CsvField(Index = 3)]
        public string Year { get; set; }
        [CsvField(Index = 4)]
        public string Make { get; set; }
        [CsvField(Index = 5)]
        public string Model { get; set; }
        [CsvField(Index = 6)]
        public string Trim { get; set; }
        [CsvField(Index = 7)]
        public string FreeText { get; set; }
        [CsvField(Index = 8)]
        public string Body { get; set; }
        [CsvField(Index = 9)]
        public string Mileage { get; set; }
        [CsvField(Index = 10)]
        public string PriceCurrent { get; set; }
        [CsvField(Index = 11)]
        public string Reserved { get; set; }
        [CsvField(Index = 12)]
        public string PriceWholesale { get; set; }
        [CsvField(Index = 13)]
        public string PriceCost { get; set; }
        [CsvField(Index = 14)]
        public string Title { get; set; }
        [CsvField(Index = 15)]
        public string Condition { get; set; }
        [CsvField(Index = 16)]
        public string Exterior { get; set; }
        [CsvField(Index = 17)]
        public string Interior { get; set; }
        [CsvField(Index = 18)]
        public string Doors { get; set; }
        [CsvField(Index = 19)]
        public string Engine { get; set; }
        [CsvField(Index = 20)]
        public string Tramsission { get; set; }
        [CsvField(Index = 21)]
        public string FuelType { get; set; }
        [CsvField(Index = 22)]
        public string DriveType { get; set; }
        [CsvField(Index = 23)]
        public string Options { get; set; }
        [CsvField(Index = 24)]
        public string Warranty { get; set; }
        [CsvField(Index = 25)]
        public string Description { get; set; }
        [CsvField(Index = 26)]
        public string Pics { get; set; }
        [CsvField(Index = 27)]
        public string DateInStock { get; set; }
        [CsvField(Index = 28)]
        public string DealerEmail { get; set; }
        [CsvField(Index = 29)]
        public string DealerName { get; set; }
        [CsvField(Index = 30)]
        public string DealerCompany { get; set; }
        [CsvField(Index = 31)]
        public string DealerAddress { get; set; }
        [CsvField(Index = 32)]
        public string DealerCity { get; set; }
        [CsvField(Index = 33)]
        public string DealerState { get; set; }
        [CsvField(Index = 34)]
        public string DealerZip { get; set; }
        [CsvField(Index = 35)]
        public string DealerTel { get; set; }
    }
}