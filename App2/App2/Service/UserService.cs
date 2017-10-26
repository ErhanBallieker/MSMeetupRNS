using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MeetupTest1.Models;
using MeetupTest1.Service.Endpoint;
using Akavache;
using Plugin.Connectivity;
using Polly;
using Refit;
using System.Reactive;
using System.Reactive.Linq;
using System.Net;

namespace MeetupTest1.Service
{
    public class UserService : IUserService
    {
        private readonly IApiRequest<IRandomUserEndpoint> _userEnpoint;

        public UserService(IApiRequest<IRandomUserEndpoint> userEndpoint)
        {
            _userEnpoint = userEndpoint;
        }

        public async Task<Unit> DeleteCache<T>() where T : new()
        {
            var result = await BlobCache.LocalMachine.InvalidateAllObjects<T>();
            return result;
        }

        public async Task<UserResponseModel> GetUserList(PriorityType priorityType, int results, int page)
        {
            var cacheKey = $"userRespo321213nse_{page}";
            var cachedCorporateList = BlobCache.LocalMachine.GetOrFetchObject(cacheKey, async () => await GetUserListAsync(priorityType, results, page), DateTimeOffset.Now.AddDays(7));
            UserResponseModel userResponseList = await cachedCorporateList.FirstOrDefaultAsync();

            if (userResponseList == null)
            {
                await BlobCache.LocalMachine.InvalidateObject<UserResponseModel>(cacheKey);
            }

            return userResponseList;
        }

        private async Task<UserResponseModel> GetUserListAsync(PriorityType priorityType, int results, int page)
        {
            UserResponseModel userResponseList = null;

            var apiRequestelector = new ApiRequestSelector<IRandomUserEndpoint>(_userEnpoint, priorityType);

            //Policy.Timeout(20);
            Policy.Bulkhead(10, 10);
            userResponseList = await Policy
                                    .Handle<ApiException>(exception => exception.StatusCode == HttpStatusCode.ServiceUnavailable)
                                    //.CircuitBreakerAsync(2,TimeSpan.FromSeconds(5))
                                    //.AdvancedCircuitBreaker(failureThreshold: 0.5,
                                    //                        samplingDuration: TimeSpan.FromSeconds(10),
                                    //                        minimumThroughput: 1,
                                    //                        durationOfBreak: TimeSpan.FromSeconds(30))
                                    //.FallbackAsync((t) => Task.FromResult(new UserResponseModel()))
                                    .WaitAndRetryAsync(5, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                                    .ExecuteAsync(async () => await apiRequestelector.GetApiRequestByPriority()?.GetUserList(results, page));

            return userResponseList;
        }
    }
}
