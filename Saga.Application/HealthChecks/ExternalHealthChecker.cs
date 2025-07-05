using SagaCoordinator.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.HealthChecks
{
    public class ExternalHealthChecker(HttpClient httpClient) : IExternalHealthChecker
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<bool> CheckHealthAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/health/system");
                if(!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Health check failed with status code: " + response.StatusCode);
                    return false;
                }
                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = await JsonDocument.ParseAsync(stream);
                var healthStatus = reader.RootElement.GetProperty("status").GetString();
                if (healthStatus != "healthy")
                {
                    Console.WriteLine("Health check failed with status: " + healthStatus);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Health check failed: " + ex.ToString());
                return false;
            }
        }
    }
}
