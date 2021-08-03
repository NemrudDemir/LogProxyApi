using AutoMapper;
using LogProxyApi.Dtos.AirTableApi;
using LogProxyApi.Dtos.Receiving;
using System.Collections.Generic;

namespace LogProxyApi.Mappings
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Record, Message>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Fields["Summary"]))
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.Fields["Message"]))
                .ForMember(dest => dest.ReceivedAt,
                    opt => opt.MapFrom(src => src.CreatedTime));

            CreateMap<Message, Record>()
                .ConvertUsing((src, dest)=>
                {
                    dest = new Record();
                    var fields = new Dictionary<string, object>() {
                        { "id", src.Id},
                        { "Summary", src.Title },
                        { "Message", src.Text },
                        { "receivedAt", src.ReceivedAt }
                    };
                    dest.Fields = fields;
                    return dest;
                });

            CreateMap<PostMessage, Message>();
        }
    }
}
