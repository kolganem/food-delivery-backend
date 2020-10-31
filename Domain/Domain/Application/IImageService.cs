using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application
{
    public interface IImageService
    {
        ImageUploadResult UploadImage(IFormFile file); // IFormFile встроенный интерфейс
        string GetUrlImage(ImageUploadResult imageUploadResult);
       
    }
}
