using DependentServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependentServices.Services
{
    public class ModuleAMicroservices : IModuleAService
    {
        private readonly HttpClient _httpClient;

        public ModuleAMicroservices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetModuleA11()
        {
            var response = await _httpClient.GetAsync($"http://localhost:5000/api/moduleA11");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
