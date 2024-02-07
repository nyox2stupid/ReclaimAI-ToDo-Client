using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TodoSQLite.Models;


namespace TodoSQLite.Data
{
    public class ReclaimTaskClient
    {
        HttpClient httpClient;
        Settings settings;
        TodoItemDatabase toDoItemDatabase;

        private async Task initReclaimClient() 
        {

            settings = await new SettingsDatabase().GetSettingAsync();
            //if (settings == null) 
            //{ 
            //    return; 
            //}
            //toDoItemDatabase = new TodoItemDatabase();
            //if (settings.Token != "")
            //{
            //    initHttpClientWithOAuth(settings.Token);
            //    await syncTaskWithDB();
            //}              
        }

        private void initHttpClientWithOAuth(String _token)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        }
        public static async Task<ReclaimTaskClient> construct() {
            ReclaimTaskClient reclaimTaskClient = new ReclaimTaskClient();
            await reclaimTaskClient.initReclaimClient();
            return reclaimTaskClient;
        }
        public async void getReclaimTask(string _taskId) {

            try
            {
                using HttpResponseMessage response = await httpClient.GetAsync("https://api.app.reclaim.ai/api/tasks/" + _taskId);
                response.EnsureSuccessStatusCode();
                TodoItem todoItem = await response.Content.ReadFromJsonAsync<TodoItem>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
        }

        public async Task syncTaskWithDB()
        {

            try
            {
                using HttpResponseMessage response = await httpClient.GetAsync("https://api.app.reclaim.ai/api/tasks");
                response.EnsureSuccessStatusCode();
                var test = response.Content.ReadFromJsonAsAsyncEnumerable<TodoItem>();
                await foreach (TodoItem todo in test)
                {
                    await toDoItemDatabase.SaveItemAsync(todo);   
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
        }

        public void updateReclaimTask() { }

        public void createReclaimTask() { }


    }
}
