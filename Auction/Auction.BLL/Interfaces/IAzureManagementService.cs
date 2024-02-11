using Auction.Common.Dtos.File;

namespace Auction.BLL.Interfaces;

public interface IAzureManagementService
{
    Task<FileDto> AddFileToBlobStorage(NewFileDto newFileDto);
    Task DeleteFromBlob(FileDto fileDto);
}
