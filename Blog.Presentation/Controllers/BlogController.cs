using Blog.Application.Services.PostService;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Presentation.Controllers
{
    public class BlogController : Controller
    {
        private readonly IPostService _postService;

        public BlogController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var allPosts = await _postService.GetAllPostsAsync();
            return View(allPosts);
        }
    }
}
