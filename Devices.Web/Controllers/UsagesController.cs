using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Devices.Core.Entities;
using Devices.Persistence;
using Devices.Core.Contracts;
using Devices.Web.ViewModels;
using Devices.Web.DataTransferObjects;

namespace Devices.Web.Controllers
{
    public class UsagesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private string _homeControllerRoute = null;

        public UsagesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _homeControllerRoute = nameof(HomeController);
            _homeControllerRoute = _homeControllerRoute.Substring(0, _homeControllerRoute.Length - 10);
        }

        private async Task InjectPeopleAndDevicesIntoViewData(Usage usage)
        {
            var people = await _unitOfWork.PersonRepository.GetAllAsync();
            var devices = await _unitOfWork.DeviceRepository.GetAllAsync();
            ViewData["DeviceId"] = new SelectList(devices, "Id", "Name", usage != null ? usage.DeviceId : devices.First().Id);
            ViewData["PersonId"] = new SelectList(people, "Id", "FullName", usage != null ? usage.PersonId : people.First().Id);
        }

        // GET: Usages
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.UsageRepository.GetAllAsync());
        }

        // GET: Usages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usage = await _unitOfWork.UsageRepository
                .FindByIdAsync(id.Value);
            if (usage == null)
            {
                return NotFound();
            }

            return View(usage);
        }

        // GET: Usages/Create
        public async Task<IActionResult> Create()
        {
            await InjectPeopleAndDevicesIntoViewData(null);
            return View();
        }

        // POST: Usages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("From,To,DeviceId,PersonId")] Usage usage)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.UsageRepository.AddAsync(usage);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index), _homeControllerRoute);
            }
            await InjectPeopleAndDevicesIntoViewData(usage);
            return View(usage);
        }

        // GET: Usages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usage = await _unitOfWork.UsageRepository.FindByIdAsync(id.Value);
            if (usage == null)
            {
                return NotFound();
            }

            await InjectPeopleAndDevicesIntoViewData(usage);
            return View(usage);
        }

        // POST: Usages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RowVersion,From,To,DeviceId,PersonId")] Usage usage)
        {
            if (id != usage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.UsageRepository.Update(usage);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsageExists(usage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), _homeControllerRoute);
            }

            await InjectPeopleAndDevicesIntoViewData(usage);
            return View(usage);
        }

        // GET: Usages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usage = await _unitOfWork.UsageRepository
                .FindByIdAsync(id.Value);
            if (usage == null)
            {
                return NotFound();
            }

            return View(usage);
        }

        // POST: Usages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usage = await _unitOfWork.UsageRepository.FindByIdAsync(id);
            _unitOfWork.UsageRepository.Delete(usage);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index), _homeControllerRoute);
        }

        private bool UsageExists(int id)
        {
            return _unitOfWork.UsageRepository.FindByIdAsync(id) != null;
        }
    }
}
