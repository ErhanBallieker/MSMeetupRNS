using MeetupTest1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeetupTest1.Service
{
    public interface IUserService
    {
        Task<UserResponseModel> GetUserList(PriorityType priorityType, int results, int page);

    }
}
