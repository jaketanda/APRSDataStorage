using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class DataProcessor
    {
        public static async Task<DataModel.Rootobject> LoadData(string callsign = "", string what = "")
        {
            string url = APIHelper.APIClient.BaseAddress + "name=" + callsign + "&what=" + what + "&apikey=127533.oPqhZ0zVA7WTvW&format=json";

            using (HttpResponseMessage response = await APIHelper.APIClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("successful call");
                    DataModel.Rootobject data = await response.Content.ReadAsAsync<DataModel.Rootobject>();

                    return data;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
