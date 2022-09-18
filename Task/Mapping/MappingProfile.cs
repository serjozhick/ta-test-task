using AutoMapper;
using TATask.AssetApi.Dto;
using TATask.Contracts;

namespace TATask.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AssetApi.Dto.Asset, Contracts.Asset>();
            CreateMap<Market, AssetPrice>()
                .ForMember(dst => dst.Market,
                    map => map.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.Price,
                    map => map.MapFrom(src => src.Ticker != null ? src.Ticker.Price : (decimal?) null));
        }
    }
}