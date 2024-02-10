using Auction.Common.Dtos.User;
using Auction.DAL.Entities;
using AutoMapper;

namespace Auction.BLL.Mappers
{
	public class DataMapperProfile : Profile
	{
		public DataMapperProfile()
		{
			ConfigureUserMapper();
		}

		private void ConfigureUserMapper()
		{
			CreateMap<User, UserDto>().ReverseMap();
			CreateMap<SignInUserDto, User>().ReverseMap();
			CreateMap<SignUpUserDto, User>().ReverseMap();
		}
	}
}
