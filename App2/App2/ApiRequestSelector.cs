using MeetupTest1.Models;
using MeetupTest1.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetupTest1
{
    public class ApiRequestSelector<T>
    {
        private readonly PriorityType _priority;
        private readonly IApiRequest<T> _apiRequest;

        public ApiRequestSelector(IApiRequest<T> apiEndpoint, PriorityType priority)
        {
            _priority = priority;
            _apiRequest = apiEndpoint;
        }

        public T GetApiRequestByPriority()
        {
            T apiRequest;

            switch (_priority)
            {
                case PriorityType.Speculative:
                    apiRequest = _apiRequest.Speculative;
                    break;
                case PriorityType.UserInitiated:
                    apiRequest = _apiRequest.UserInitiated;
                    break;
                case PriorityType.Background:
                    apiRequest = _apiRequest.Background;
                    break;
                default:
                    apiRequest = _apiRequest.UserInitiated;
                    break;
            }

            return apiRequest;
        }
    }
}
