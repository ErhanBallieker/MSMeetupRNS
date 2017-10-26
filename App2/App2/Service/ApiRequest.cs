using Fusillade;
using ModernHttpClient;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MeetupTest1.Service
{
    public class ApiRequest<T> : IApiRequest<T>
    {
        private readonly Lazy<T> _background;
        private readonly Lazy<T> _userInitiated;
        private readonly Lazy<T> _speculative;

        public string BaseApiAddress = "https://randomuser.me/api";

        public ApiRequest()
        {
            _background = new Lazy<T>(() => CreateClient(new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Background), BaseApiAddress));
            _userInitiated = new Lazy<T>(() => CreateClient(new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.UserInitiated), BaseApiAddress));
            _speculative = new Lazy<T>(() => CreateClient(new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Speculative), BaseApiAddress));
        }

        public T Background => _background.Value;

        public T Speculative => _speculative.Value;

        public T UserInitiated => _userInitiated.Value;

        public T CreateClient(HttpMessageHandler handler, string baseApiAddress = null)
        {
            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseApiAddress ?? BaseApiAddress)
            };

            var res = RestService.For<T>(client);
            return res;
        }
    }
}
