using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Application;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class ImageService : IImageService
    {
        private const string CLOUD_Name = "dpcnhdz4b";
        private const string API_KEY = "117934724393154";
        private const string API_Secret = "wmsoOwDczDcniO3ZU7NtHyCwGjk";

        private readonly Cloudinary _cloudinary;

        public ImageService()
        {
            Account account = new Account(CLOUD_Name, API_KEY, API_Secret);
            _cloudinary = new Cloudinary(account);
        }
        
        public string GetUrlImage(ImageUploadResult imageUploadResult)
        {            
            return imageUploadResult.Url.ToString();
        }

        public ImageUploadResult UploadImage(IFormFile file) 
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName)
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            return uploadResult;
        }

        public List<Resource> GetAllResourseImages()
        {
            var listResourses = _cloudinary.ListResources();
            
            return listResourses.Resources.ToList(); 
        }
    }
}
