using Blog.API.Models.Domain;
using Blog.API.Models.DTO;
using Blog.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //GET: {apibaseURL}/api/images 

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            //call image repository to get all images
            var images = await imageRepository.GetAll();

            //Convert to DTO
            var resposne = new List<BlogImageDto>();
            foreach (var image in images)
            {
                resposne.Add(new BlogImageDto
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Url = image.Url,
                });
            }

            return Ok(resposne);
        }

        //POST: {apibaseurl}/api/images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title) 
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid) 
            {
                //File upload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,
                };

                await imageRepository.Upload(file, blogImage);

                //convert to Model to DTO

                var response = new BlogImageDto
                {
                    Id=blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url,
                };

                return Ok(blogImage);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file) 
        {
            var allowedExtensions = new string[] {".jpg",".jpeg",".png"};
            if (allowedExtensions.Contains(Path.GetExtension(file.Name).ToLower())) 
            {
                ModelState.AddModelError("file", "Unsupported fie format");
            }

            if (file.Length > 10485760) 
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }

  
    }
}
