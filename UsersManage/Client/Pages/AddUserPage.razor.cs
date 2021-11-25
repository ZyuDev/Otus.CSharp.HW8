using BBComponents.Enums;
using BBComponents.Services;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace UsersManage.Client.Pages
{
    public partial class AddUserPage : ComponentBase
    {
        private bool _isWaiting;
        private Customer _item = new Customer();

        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public IAlertService AlertService { get; set; }

        private async Task OnAddClick()
        {
            if (!Validate())
            {
                AlertService.Add("Please fill empty fields.", BootstrapColors.Danger);
                return;
            }

            _isWaiting = true;
            try
            {
                var json = JsonConvert.SerializeObject(_item);
                var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await Http.PostAsync($"Api/Users/", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    AlertService.Add($"Item added", BootstrapColors.Success);
                }
                else
                {
                    AlertService.Add($"Cannot add item. {(int)response.StatusCode} {response.ReasonPhrase}", BootstrapColors.Danger);
                }
            }
            catch (Exception e)
            {
                AlertService.Add($"Cannot add item. Message: {e.Message}", BootstrapColors.Danger);
            }
            finally
            {
                _isWaiting = false;
            }


        }

        private bool Validate()
        {
            var isValid = true;

            if (string.IsNullOrEmpty(_item.FullName))
            {
                isValid = false;
            }

            if (string.IsNullOrEmpty(_item.Email))
            {
                isValid = false;
            }

            if (string.IsNullOrEmpty(_item.Phone))
            {
                isValid = false;
            }

            return isValid;
        }

    }
}
