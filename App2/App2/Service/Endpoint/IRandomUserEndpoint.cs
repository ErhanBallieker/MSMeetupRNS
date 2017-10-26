using MeetupTest1.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using ModernHttpClient;

namespace MeetupTest1.Service.Endpoint
{
    [Headers("Content-Type : application/json")]
    public interface IRandomUserEndpoint
    {
        [Get("/")]
        Task<UserResponseModel> GetUserList(int results, int page);
    }

}
