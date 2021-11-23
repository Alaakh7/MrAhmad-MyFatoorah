using RestSharp;
namespace MrAhmad_MyFatoorah.Services
{
    public class BulkSMS
    {
        public static string URL = ""; //Set Here You API BULK SMS

        public static string SendMessage(string values)
        {
            RestClient restClient = new RestClient(URL);
            RestRequest restRequest = new RestRequest($"phone={values.Split(',')[0]}&message={values.Split(',')[2]}", Method.GET, DataFormat.Json);
            IRestResponse restResponse = restClient.Execute(restRequest);
            if (restResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception(restResponse.Content);
            }
            else
                return restResponse.Content;
        }
    }
}