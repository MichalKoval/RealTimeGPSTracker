using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands
{
    public class AuthorizedCommand<T> : IRequest<T> where T : class
    {
        public string OwnerId { get; set; } = null;
    }
}
