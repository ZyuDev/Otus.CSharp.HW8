using BBComponents.Enums;
using BBComponents.Services;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace UsersManage.Client.Pages
{
    public partial class FindUserPage: ComponentBase
    {
        private bool _isWaiting;
        private int _userId;
        private Customer _item;

        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public IAlertService AlertService { get; set; }


        private async Task OnGetUserClick()
        {
            _isWaiting = true;
            _item = null;
            try
            {
                var response = await Http.GetAsync($"Api/Users/{_userId}");
                

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _item = JsonConvert.DeserializeObject<Customer>(content,
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });


                }
                else
                {
                    AlertService.Add($"Cannot get item. {(int)response.StatusCode} {response.ReasonPhrase}", BootstrapColors.Danger);
                }
            }
            catch (Exception e)
            {
                AlertService.Add($"Cannot get item. Message: {e.Message}", BootstrapColors.Danger);
            }
            finally
            {
                _isWaiting = false;
            }

            

        }

    }
}
