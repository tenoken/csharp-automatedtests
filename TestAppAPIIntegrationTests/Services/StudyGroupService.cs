using RestSharp;
using TestAppAPIIntegrationTests.Services.Interfaces;

namespace TestAppAPIIntegrationTests.Services
{
    internal class StudyGroupService : IStudyGroupService
    {
        private readonly string _baseUrl = "https://localhost:7058";

        public RestResponse CreateStudyGroup(string json)
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest("/createstudygroup", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = json;
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }

        public RestResponse GetStudyGroups()
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest("/getstudygroups", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            //request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }

        public RestResponse JoinStudyGroup(int userId, int studyGroupId)
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest($"/joinstudygroup?studyGroupId={studyGroupId}&userId={userId}", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            //var body = json;
            request.AddParameter("application/json", "", ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }

        public RestResponse LeaveStudyGroup(int userId, int studyGroupId)
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest($"/leavestudygroup?studyGroupId={studyGroupId}&userId={userId}", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            //var body = json;
            request.AddParameter("application/json", "", ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }

        public RestResponse SearchGroupByDate(DateTime convertedDate, bool sorted)
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest($"/searchstudygroups?from={convertedDate}&orderByMostRecent={sorted}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            //var body = json;
            //request.AddParameter("application/json", "", ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }

        public RestResponse SearchGroupBySubject(string subject)
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest($"/searchstudygroups/{subject}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            //var body = json;
            //request.AddParameter("application/json", "", ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }
    }
}
