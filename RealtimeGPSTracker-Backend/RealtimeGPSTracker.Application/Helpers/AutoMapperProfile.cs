using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using RealtimeGpsTracker.Core.Commands.AuthCommands;
using RealtimeGpsTracker.Core.Commands.CoordinateCommands;
using RealtimeGpsTracker.Core.Commands.DeviceCommands;
using RealtimeGpsTracker.Core.Commands.UserCommands;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using RealtimeGpsTracker.Core.Dtos.Responses.UserResponses;
using RealtimeGpsTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using static RealtimeGpsTracker.Core.Commands.CoordinateCommands.InsertCoordinatesCommand;

namespace RealtimeGPSTracker.Application.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /// Auth mapping ---------------------------------------------------------------
            CreateMap<RegisterUserCommand, User>();
            
            /// User -----------------------------------------------------------------------
            // User detail mapping
            CreateMap<User, DetailUserResponse>();

            // User update command mapping
            CreateMap<UpdateUserDetailsCommand, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            /// Device ---------------------------------------------------------------------
            // Device create command and reponse mapping
            CreateMap<CreateDeviceCommand, GpsDevice>();
            CreateMap<GpsDevice, CreateDeviceResponse>();

            // Device update command mapping
            CreateMap<UpdateDeviceCommand, GpsDevice>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Interval, opt => opt.MapFrom(src => src.Interval))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

            /// Coordinate -----------------------------------------------------------------
            // Coordinates insert command and list of Db gps corrdinates mapping
            CreateMap<InsertCoordinatesCommand, IList<GpsCoordinate>>()
                .ConvertUsing<InsertCoordinatesCommandConverter>();
        }
    }
}
