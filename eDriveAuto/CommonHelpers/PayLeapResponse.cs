using System;

namespace Edrive.CommonHelpers
{
	public class PayLeapResponse
	{
		public string AuthCode;
		public string ExtData;
		public string GetAvsResult;
		public string GetAVSResultTxt;
		public string GetCommercialCard;
		public string GetCvResult;
		public string GetCvResultTxt;
		public string GetStreetMatchTxt;
		public string GetZipMatchTxt;
		public string HostCode;
		public string Message;
		public string Message1;
		public string Message2;
		public int PnRef;
		public string RespMsg;
		public int Result = -1;

		public void Fill(string xmlString)
		{
			// parse and interpret response
			this.AuthCode = RetrieveXMLString(xmlString, "</AuthCode>");
			this.ExtData = RetrieveXMLString(xmlString, "</ExtData>");
			this.GetAvsResult = RetrieveXMLString(xmlString, "</GetAVSResult>");
			this.GetAVSResultTxt = RetrieveXMLString(xmlString, "</GetAVSResultTXT>");
			this.GetCommercialCard = RetrieveXMLString(xmlString, "</GetCommercialCard>");
			this.GetCvResult = RetrieveXMLString(xmlString, "</GetCVResult>");
			this.GetCvResultTxt = RetrieveXMLString(xmlString, "</GetCVResultTXT>");
			this.GetStreetMatchTxt = RetrieveXMLString(xmlString, "</GetStreetMatchTXT>");
			this.GetZipMatchTxt = RetrieveXMLString(xmlString, "</GetZipMatchTXT>");
			this.HostCode = RetrieveXMLString(xmlString, "</HostCode>");
			this.Message = RetrieveXMLString(xmlString, "</Message>");
			this.Message1 = RetrieveXMLString(xmlString, "</Message1>");
			this.Message2 = RetrieveXMLString(xmlString, "</Message2>");
			int.TryParse(RetrieveXMLString(xmlString, "</PNRef>"), out this.PnRef);
			this.RespMsg = RetrieveXMLString(xmlString, "</RespMSG>");
			int.TryParse(RetrieveXMLString(xmlString, "</Result>"), out this.Result);
		}

		public string RetrieveXMLString(String s1, String s2)
		{
			String retrieveXMLString = String.Empty;
			int x = s1.IndexOf(s2);

			for(int i = x; i >= 1; i--)
			{
				if(s1.Substring(i, 1) == ">")
				{
					retrieveXMLString = s1.Substring(i + 1, x - i - 1).Trim();
					break;
				}
			}
			return retrieveXMLString;
		}

        public string Response()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine("Auth Code:               " + this.AuthCode);
            sb.AppendLine("Ext Data:                " + this.ExtData);
            sb.AppendLine("Get AVS Result:          " + this.GetAvsResult);
            sb.AppendLine("Get AVS Result Text:     " + this.GetAVSResultTxt);
            sb.AppendLine("Get Commercial Card:     " + this.GetCommercialCard);
            sb.AppendLine("Get CV Result:           " + this.GetCvResult);
            sb.AppendLine("Get CV Result Text:      " + this.GetCvResultTxt);
            sb.AppendLine("Get Street Match Text:   " + this.GetStreetMatchTxt);
            sb.AppendLine("Get Zip Match Text:      " + this.GetZipMatchTxt);
            sb.AppendLine("Host Code:               " + this.HostCode);
            sb.AppendLine("Message:                 " + this.Message);
            sb.AppendLine("Message1:                " + this.Message1);
            sb.AppendLine("Message2:                " + this.Message2);
            sb.AppendLine("PN Ref:                  " + this.PnRef);
            sb.AppendLine("Resp Message:            " + this.RespMsg);
            sb.AppendLine("Result:                  " + this.Result);
        
            return sb.ToString();
        }
	}
}