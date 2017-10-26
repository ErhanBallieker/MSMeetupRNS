using MeetupTest1.Models;
using MeetupTest1.Service;
using MeetupTest1.Service.Endpoint;
using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Result> UserList { get; set; }
        public MainPage()
        {
            InitializeComponent();

            //basic client call.
            //Task.Run(async () =>
            //{
            //    using (var client = new HttpClient())
            //    {
            //        client.BaseAddress = new Uri("https://randomuser.me/api");
            //        var content = await client.GetStringAsync("?results=10&page=1");
            //        var result = JsonConvert.DeserializeObject<UserResponseModel>(content);

            //        Device.BeginInvokeOnMainThread(() =>
            //        {
            //            UserList = new ObservableCollection<Result>();
            //            if (result != null && result.results.Count > 0)
            //            {
            //                foreach (var item in result.results)
            //                {
            //                    UserList.Add(item);
            //                }
            //            }
            //            lstView.ItemsSource = UserList;
            //        });
            //    }
            //});
            var httpClient = new HttpClient(new NativeMessageHandler());
            //structered call
            Task.Run(async () =>
            {
                var service = new UserService(new ApiRequest<IRandomUserEndpoint>());
                var result = await service.GetUserList(PriorityType.Speculative, 10, 1);

                Device.BeginInvokeOnMainThread(() =>
                {
                    UserList = new ObservableCollection<Result>();
                    if (result != null && result.results.Count > 0)
                    {
                        foreach (var item in result.results)
                        {
                            UserList.Add(item);
                        }
                    }
                    lstView.ItemsSource = UserList;
                });
            });
        }
    }
}
