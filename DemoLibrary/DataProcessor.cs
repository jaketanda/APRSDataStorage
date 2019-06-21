using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DemoLibrary
{
    public class DataProcessor
    {
        public static async Task<DataModel> LoadData(string callsign = "")
        {
            string url = APIHelper.APIClient.BaseAddress + "name=" + callsign + "&what=loc&apikey=127533.oPqhZ0zVA7WTvW&format=json";

            using (HttpResponseMessage response = await APIHelper.APIClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    DataResultModel result = await response.Content.ReadAsAsync<DataResultModel>();

                    return result.Results;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
