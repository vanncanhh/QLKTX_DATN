using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TECH
{
    public class VnPayLibrary
    {
        public VnPayLibrary() { }

        private readonly SortedList<string, string> requestData = new SortedList<string, string>();
        private readonly SortedList<string, string> responseData = new SortedList<string, string>();

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            return responseData.ContainsKey(key) ? responseData[key] : string.Empty;
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            string signData = CreateQueryString(requestData); // key=value nối bằng & theo thứ tự
            string secureHash = HmacSHA512(vnp_HashSecret, signData);

            // B2: Tạo query string đã URL-encode
            var query = new StringBuilder();
            foreach (var kv in requestData)
            {
                query.Append(HttpUtility.UrlEncode(kv.Key) + "=" + HttpUtility.UrlEncode(kv.Value) + "&");
            }
            Console.WriteLine("SIGN DATA: " + signData);
            Console.WriteLine("HASH: " + secureHash);

            // B3: Bổ sung vnp_SecureHashType và vnp_SecureHash (KHÔNG encode hash)
            query.Append("vnp_SecureHashType=SHA512");
            query.Append("&vnp_SecureHash=" + secureHash);



            return baseUrl + "?" + query.ToString();
        }

        public bool ValidateSignature(string vnp_HashSecret)
        {
            string signData = CreateQueryString(responseData);
            string vnp_SecureHash = GetResponseData("vnp_SecureHash");

            string calculatedHash = HmacSHA512(vnp_HashSecret, signData);

            return calculatedHash.Equals(vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string CreateQueryString(SortedList<string, string> data)
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in data)
            {
                if (!kv.Key.Equals("vnp_SecureHash", StringComparison.InvariantCultureIgnoreCase))
                {
                    sb.Append(kv.Key + "=" + kv.Value + "&");
                }
            }
            return sb.ToString().TrimEnd('&');
        }


        private string HmacSHA512(string key, string inputData)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
                var hex = new StringBuilder(hashValue.Length * 2);
                foreach (byte b in hashValue)
                {
                    hex.AppendFormat("{0:x2}", b);
                }
                return hex.ToString();
            }
        }
    }
}
