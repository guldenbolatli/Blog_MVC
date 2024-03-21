using Blog.Application.Services.PostService;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Presentation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class PanelController : Controller
    {
        private readonly IPostService _postService;

        public PanelController(IPostService postService)
        {
            _postService = postService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async  Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePostAsync(id);
            return Redirect("~/Blog");
        }

    }
}
