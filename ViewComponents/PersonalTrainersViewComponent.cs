using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.ViewComponents
{
    public class PersonalTrainersViewComponent : ViewComponent
    {
        private readonly IPersonalTrainerRepository _personalTrainerRepository;

        public PersonalTrainersViewComponent(IPersonalTrainerRepository personalTrainerRepository)
        {
            _personalTrainerRepository = personalTrainerRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var trainers = await _personalTrainerRepository.GetAllWithUsersAsync();
            var trainerViewModels = trainers.Select(t => new PersonalTrainerViewModel
            {
                FirstName = t.User.FirstName,
                LastName = t.User.LastName,
                ImageId = t.User.ProfilePictureId ?? Guid.Empty 
            })
            .Take(6) 
            .ToList();

            return View(trainerViewModels);
        }
    }
}
