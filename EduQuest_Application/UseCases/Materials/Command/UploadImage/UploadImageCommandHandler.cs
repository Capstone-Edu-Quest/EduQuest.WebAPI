using Azure.Storage.Blobs.Models;
using EduQuest_Application.Abstractions.AzureBlobStorage;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using MediatR;
using System.Net;
using System.Net.Http.Headers;

namespace EduQuest_Application.UseCases.Materials.Command.UploadImage;


public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, APIResponse>
{
    private readonly IAzureBlobStorage _azureBlobStorage;

    public UploadImageCommandHandler(IAzureBlobStorage azureBlobStorage)
    {
        _azureBlobStorage = azureBlobStorage;
    }

    public async Task<APIResponse> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        string fileName = request.ImageFile.FileName;
        var uniqueFileName = $"{Guid.NewGuid()}{fileName}";

        using var memoryStream = new MemoryStream();
        await request.ImageFile.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;
        var httpHeaders = new BlobHttpHeaders
        {
            ContentType = "image/png" // file type
        };


        await _azureBlobStorage.UploadAsync(uniqueFileName, memoryStream, httpHeaders);

        string fileUrl = _azureBlobStorage.GetFileUrl(uniqueFileName);

        return GeneralHelper.CreateSuccessResponse(
            HttpStatusCode.OK,
            $"Upload Image Sucessfully",
            new
            {
                url = fileUrl
            },
            "name",
            "image"
        );
    }
}
