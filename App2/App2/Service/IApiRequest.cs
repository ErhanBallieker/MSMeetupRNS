using System;
using System.Collections.Generic;
using System.Text;

namespace MeetupTest1.Service
{
    public interface IApiRequest<out T>
    {
        T Speculative { get; }
        T UserInitiated { get; }
        T Background { get; }
    }
}
