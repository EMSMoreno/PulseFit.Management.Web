using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;

namespace PulseFit.Management.Web.Controllers
{
    public class OnlineClassesController : Controller
    {
        private readonly IOnlineClassRepository _onlineClassRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IClientRepository _clientRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public OnlineClassesController(
            IOnlineClassRepository onlineClassRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            DataContext context,
            UserManager<User> userManager,
            IClientRepository clientRepository,
            ISubscriptionRepository subscriptionRepository
            )
        {
            _onlineClassRepository = onlineClassRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _context = context;
            _userManager = userManager;
            _clientRepository = clientRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        private async Task<bool> UserHasAccessToOnlineClasses()
        {
            // Obtém o usuário logado
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return false; 
            }

            if (await _userManager.IsInRoleAsync(user, "Admin") ||
                await _userManager.IsInRoleAsync(user, "PersonalTrainer") ||
                await _userManager.IsInRoleAsync(user, "Employee"))
            {
                return true;
            }

            var client = await _clientRepository.GetByUserIdAsync(user.Id);

            if (client == null)
            {
                return false; 
            }

            var hasAccess = client.UserSubscriptions
                .Where(us => us.Subscription.Status == SubscriptionStatus.Active)
                .Any(us => us.Subscription.IncludeOnlineClasses);

            return hasAccess;
        }

        // GET: OnlineClasses
        public async Task<IActionResult> Index()
        {
            if (!await UserHasAccessToOnlineClasses())
            {
                return RedirectToAction("AccessDenied", "Account"); // Ou outra lógica de redirecionamento
            }

            var videos = _onlineClassRepository.GetAll().ToList()
                .GroupBy(v => v.Category)
                .OrderBy(g => g.Key);

            return View(videos);
        }

        // GET: OnlineClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineClass = await _onlineClassRepository.GetByIdAsync(id.Value);
            if (onlineClass == null)
            {
                return NotFound();
            }

            return View(onlineClass);
        }

        // GET: OnlineClasses/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(Enum.GetValues(typeof(OnlineClass.ClassCategory)).Cast<OnlineClass.ClassCategory>());
            return View();
        }

        // POST: OnlineClasses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OnlineClass onlineClass)
        {
            if (ModelState.IsValid)
            {
                var youtubeData = await GetYouTubeVideoData(onlineClass.VideoUrl);
                onlineClass.Title = youtubeData.Title;
                onlineClass.ThumbnailUrl = youtubeData.ThumbnailUrl;

                await _onlineClassRepository.CreateAsync(onlineClass);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(Enum.GetValues(typeof(OnlineClass.ClassCategory)).Cast<OnlineClass.ClassCategory>());

            return View(onlineClass);
        }

        // GET: OnlineClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineClass = await _onlineClassRepository.GetByIdAsync(id.Value);
            if (onlineClass == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(Enum.GetValues(typeof(OnlineClass.ClassCategory)).Cast<OnlineClass.ClassCategory>());

            return View(onlineClass);
        }

        // POST: OnlineClasses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OnlineClass onlineClass)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var youtubeData = await GetYouTubeVideoData(onlineClass.VideoUrl);
                    onlineClass.Title = youtubeData.Title;
                    onlineClass.ThumbnailUrl = youtubeData.ThumbnailUrl;

                    await _onlineClassRepository.UpdateAsync(onlineClass);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _onlineClassRepository.ExistAsync(onlineClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(Enum.GetValues(typeof(OnlineClass.ClassCategory)).Cast<OnlineClass.ClassCategory>());

            return View(onlineClass);
        }

        // GET: OnlineClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineClass = await _onlineClassRepository.GetByIdAsync(id.Value);
            if (onlineClass == null)
            {
                return NotFound();
            }

            return View(onlineClass);
        }

        // POST: OnlineClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var onlineClass = await _onlineClassRepository.GetByIdAsync(id);
            await _onlineClassRepository.DeleteAsync(onlineClass);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ManageOnlineClasses()
        {
            var videos = _onlineClassRepository.GetAll().ToList()
                .OrderBy(c => c.Category);

            return View(videos);
        }

        private async Task<(string Title, string ThumbnailUrl)> GetYouTubeVideoData(string url)
        {
            var videoId = ExtractYouTubeVideoId(url);

            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyDHgfKrHJeyVACrUUG79LsyWcnEDVMF_Bk"
            });

            var request = youtubeService.Videos.List("snippet");
            request.Id = videoId;

            var response = await request.ExecuteAsync();
            var video = response.Items.FirstOrDefault();

            if (video == null)
                throw new Exception("Vídeo não encontrado!");

            return (video.Snippet.Title, video.Snippet.Thumbnails.Default__.Url);
        }

        private string ExtractYouTubeVideoId(string url)
        {
            var uri = new Uri(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            return query["v"];
        }
    }
}