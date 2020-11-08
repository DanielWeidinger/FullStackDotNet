using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Devices.Web.ViewModels;
using Devices.Core.Contracts;
using Devices.Core.Entities;
using Devices.Web.DataTransferObjects;

namespace Devices.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IList<Device> devices = await _unitOfWork.DeviceRepository.GetAllAsync();
            IList<UsageDto> deviceUsages;
            int selctedDeviceId = -1;
            if (devices.Any())
            {
                selctedDeviceId = devices.First().Id;
                deviceUsages = (await _unitOfWork.UsageRepository.FindByDeviceId(selctedDeviceId))
                    .Select(u => new UsageDto(u))
                    .ToList();
            }
            else
            {
                deviceUsages = new List<UsageDto>();
            }

            var homeIndexViewModel = new HomeIndexViewModel(deviceUsages, devices, selctedDeviceId);
            return View(homeIndexViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(HomeIndexViewModel viewModel)
        {
            IList<Device> devices = await _unitOfWork.DeviceRepository.GetAllAsync();
            IList<UsageDto> deviceUsages;
            int selctedDeviceId = viewModel.SelectedDeviceId;
            if (devices.Any())
            {
                deviceUsages = (await _unitOfWork.UsageRepository.FindByDeviceId(selctedDeviceId))
                    .Select(u => new UsageDto(u))
                    .ToList();
            }
            else
            {
                deviceUsages = new List<UsageDto>();
            }

            var homeIndexViewModel = new HomeIndexViewModel(deviceUsages, devices, selctedDeviceId);
            return View(homeIndexViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
