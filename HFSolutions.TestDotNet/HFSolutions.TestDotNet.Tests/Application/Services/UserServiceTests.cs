using System.Net.Http.Headers;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;
using HFSolutions.TestDotNet.Application.Dtos.UserTaskDto;
using HFSolutions.TestDotNet.Tests.Application.Interfaces;
using HFSolutions.TestDotNet.Tests.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HFSolutions.TestDotNet.Tests.Application.Services
{
    public class UserServiceTests : IUserServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public UserServiceTests(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> Login(string username, string password)
        {
            string loginUrl = $"{_httpClient.BaseAddress!.AbsoluteUri}/User/Login";

            var userLogin = new UserLoginDto
            {
                UserName = username,
                Password = password
            };

            var httpContent = userLogin.ToStringHttpContent();

            var response = await _httpClient.PostAsync(loginUrl, httpContent);

            response.EnsureSuccessStatusCode();

            string token = await response.Content.ReadAsStringAsync();

            return token;
        }

        public async Task<List<UserTaskDto>?> GetUserTasks(string username, string password)
        {
            string userTasksUrl = $"{_httpClient.BaseAddress!.AbsoluteUri}/UserTask/AllTasks";

            string token = await Login(username, password);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(userTasksUrl);

            response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();

            var userTasks = JsonConvert.DeserializeObject<List<UserTaskDto>?>(responseString);

            return userTasks;
        }
    }
}
