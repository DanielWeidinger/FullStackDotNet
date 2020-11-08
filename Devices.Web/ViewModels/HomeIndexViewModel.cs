using Devices.Core.Entities;
using Devices.Web.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devices.Web.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<UsageDto> DeviceUsages { get; set; }
        public int SelectedDeviceId { get; set; }

        public SelectList DeviceSelectedList { get; set; }

        public HomeIndexViewModel() { }

        public HomeIndexViewModel(IEnumerable<UsageDto> deviceUsages, IList<Device> devices, int selectedDeviceId)
        {
            DeviceUsages = deviceUsages;
            SelectedDeviceId = selectedDeviceId;
            DeviceSelectedList = new SelectList(devices, "Id", "Name", selectedDeviceId);
        }
    }
}
