﻿using Auction.Common.Dtos.Bid;
using Auction.Common.Dtos.Product;
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
			ConfigureProductMapper();
			ConfigureBidMapper();
		}

		private void ConfigureUserMapper()
		{
			CreateMap<User, UserDto>().ReverseMap();
			CreateMap<SignInUserDto, User>().ReverseMap();
			CreateMap<SignUpUserDto, User>().ReverseMap();
		}

		private void ConfigureProductMapper()
		{
			CreateMap<Product, ProductDto>().ReverseMap();
			CreateMap<CreateProductDto, Product>().ReverseMap();
		}

		private void ConfigureBidMapper()
		{
			CreateMap<Bid, CreateBidDto>().ReverseMap();
		}
	}
}
