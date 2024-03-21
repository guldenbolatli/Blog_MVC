using AutoMapper;
using Blog.Application.Models.DTOs;
using Blog.Application.Services.PostService;
using Blog.Domain.Entities.Concrete;
using Blog.Presentation.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Blog.Presentation.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper, UserManager<AppUser> userManager)
        {
            _postService= postService;
            _mapper= mapper;
            _userManager= userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public  IActionResult AddPost()
        {
            var postDTO= _postService.CreatePostAsync().Result;
            CreatePostVM postVM = new CreatePostVM();
     
            List<GenreDTO> genres = postDTO.Genres.ToList();
            SelectList items = new SelectList(genres, "Id", "Name");

            
            ViewBag.GenreList = items;
            return View(postVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(CreatePostVM post)
        {
            if (ModelState.IsValid)
            {
                CreatePostDTO postDTO = new CreatePostDTO();
                postDTO.Title = post.Title;
                postDTO.Content = post.Content;
                postDTO.GenreId = post.GenreId;

                string imageName = new Random().Next(1, 100000).ToString()+"_"+post.ImageFile.FileName;

                postDTO.ImagePath = imageName; 

                FileStream fs = new FileStream("wwwRoot/Images/PostImages/"+imageName, FileMode.Create);

                await post.ImageFile.CopyToAsync(fs);
                fs.Close();

                AppUser user = await _userManager.GetUserAsync(User);
                postDTO.AppUserId = user.Id;


               await _postService.CreatePostAsync(postDTO);
                return View();
            }

            return View();
        }
    }
}
