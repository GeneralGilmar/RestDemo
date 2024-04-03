using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RESTDemo
{
    public class MainViewModel
    {
        HttpClient client;
        JsonSerializerOptions _options;
        string baseUrl = "https://660d566c6ddfa2943b342521.mockapi.io";
        private List<User>? _users;
        public MainViewModel()
        {
            client = new HttpClient();
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

        }

        public ICommand GetAllUsersCommand =>
            new Command(async () =>
            {
                var url = $"{baseUrl}/users";
                var response= await client.GetAsync(url);

                if(response.IsSuccessStatusCode)
                {
                    //var content = await response.Content.ReadAsStringAsync();
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        var data = await JsonSerializer.DeserializeAsync<List<User>>(responseStream, _options);
                        _users = data;
                    }
                }

            });

        public ICommand GetSingleUserCommand =>
            new Command(async () =>
            {
                var url = $"{baseUrl}/users/2";
                var response = await client.GetStringAsync(url);
            });

        public ICommand AddUserCommand =>
            new Command(async () =>
            {
                var url = $"{baseUrl}/users";
                var user = new User
                {
                    createdAt = DateTime.Now,
                    name = "Gilmar David",
                    avatar = "https://www.gyltech.co.ao"
                };

                string json = JsonSerializer.Serialize<User>(user,_options);

                StringContent content = new StringContent(json,Encoding.UTF8,"application/json");

                var response = await client.PostAsync(url, content);  
            });

       public ICommand UpdateUserCommand =>
       new Command(async () =>
       {

           var user = _users?.FirstOrDefault(x=> x.id=="1");

           if (user != null)
           {
               user.name = "Valdemar David";

               var url = $"{baseUrl}/users/1";

               string json = JsonSerializer.Serialize<User>(user, _options);

               StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

               var response = await client.PutAsync(url, content);
           }
       });

        public ICommand DeleteUserCommand =>
            new Command(async () =>
            {
                var url = $"{baseUrl}/users/10";

                var response = await client.DeleteAsync(url);

            });


    }
}
