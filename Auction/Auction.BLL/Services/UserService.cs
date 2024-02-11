using Auction.BLL.Interfaces;
using Auction.BLL.Services.Abstract;
using Auction.Common.Dtos.File;
using Auction.Common.Dtos.User;
using Auction.Common.Response;
using Auction.DAL.Context;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Auction.BLL.Services;

public class UserService : BaseService, IUserService
{
    private readonly IAzureManagementService _azureManagementService;

	public UserService(AuctionContext context, IMapper mapper, IAzureManagementService azureManagementService) : base(context, mapper)
	{
        _azureManagementService = azureManagementService;
	}

    public async Task<Response<bool>> DeleteUser(Guid userId)
    {
        var response = new Response<bool>()
        {
            Status = Status.Error,
        };

        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id.Equals(userId)) ?? throw new ArgumentNullException("User was not found");

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        response.Status = Status.Success;
        return response;
    }

    public async Task<Response<UserDto>> UpdateUser(EditUserDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

        if (user is null)
        {
            return new Response<UserDto>
            {
                Message = $"User with email {userDto.Email} not found",
                Status = Status.Error
            };
        }

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Email = userDto.Email;
        user.Phone = userDto.Phone;
        user.UpdatedAt = DateTime.UtcNow;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var userResponse = _mapper.Map<UserDto>(user);

        return new Response<UserDto>()
        {
            Value = userResponse,
            Message = "You have updated user succesfully",
            Status = Status.Success
        };
    }

    public async Task<Response<UserDto>> UpdatePhoto(IFormFile file, Guid userId)
    {
        var fileDto = new NewFileDto()
        {
            Stream = file.OpenReadStream(),
            FileName = file.FileName
        };

        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id.Equals(userId)) ?? throw new ArgumentNullException("User was not found");

        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            var oldFile = new FileDto()
            {
                Url = user.AvatarUrl
            };

            await _azureManagementService.DeleteFromBlob(oldFile);
        }

        var addedFile = await _azureManagementService.AddFileToBlobStorage(fileDto);

        user.AvatarUrl = addedFile.Url;

        _context.Users.Update(user);

        await _context.SaveChangesAsync();

        var userResponse = _mapper.Map<UserDto>(user);

        return new Response<UserDto>()
        {
            Value = userResponse,
            Message = "You have updated user photo succesfully",
            Status = Status.Success
        };
    }

    public async Task<Response<UserDto>> DeletePhoto(FileDto fileDto, Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id.Equals(userId)) ?? throw new ArgumentNullException("User was not found");

        await _azureManagementService.DeleteFromBlob(fileDto);

        user.AvatarUrl = string.Empty;

        _context.Users.Update(user);

        await _context.SaveChangesAsync();

        var userResponse = _mapper.Map<UserDto>(user);

        return new Response<UserDto>()
        {
            Value = userResponse,
            Message = "You have deleted user photo succesfully",
            Status = Status.Success
        };
    }
}
