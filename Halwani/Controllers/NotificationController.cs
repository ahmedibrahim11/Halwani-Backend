using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GenericModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Halwani.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IStringLocalizer<NotificationController> _localizer;
        private INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository NotificationRepository, IStringLocalizer<NotificationController> localizer)
        {
            _localizer = localizer;
            _notificationRepository = NotificationRepository;
        }

        [HttpPost]
        [Route("List")]
        public IActionResult List(PaginationViewModel model)
        {
            try
            {
                var result = _notificationRepository.List(model, User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (result == null)
                    return Problem("");

                result.PageData.ForEach(e => e.Text = string.Format(_localizer.GetString(e.ResourceKey), e.MadeByName));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
