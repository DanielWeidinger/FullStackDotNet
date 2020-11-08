using System.Linq;
using System.Threading.Tasks;
using Devices.Core.Contracts;
using Devices.Core.DataTransferObjects;
using Devices.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace WebAPI.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeviceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DeviceDto[]>> GetAsync()
        {
            return (await _unitOfWork.DeviceRepository.GetAllAsync()).Select(device => new DeviceDto(device)).ToArray();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDto>> GetByIdAsync(int id) => new DeviceDto((await _unitOfWork.DeviceRepository.FindByIdAsync(id)));
        

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<DeviceDto>> AddAsync(DeviceDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //tried reflection approach
            Device newDevice = dto.SetProps<DeviceDto, Device>();

            /*Device newDevice = new Device
            {
                Name = dto.Name,
                DeviceType = dto.DeviceType,
                SerialNumber = dto.SerialNumber,
            };*/

            var addedDevice = await _unitOfWork
                .DeviceRepository.AddAsync(newDevice);
            await _unitOfWork.SaveChangesAsync();

            return new DeviceDto(addedDevice);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DeviceDto>> EditAsync(int id, [FromBody] DeviceDto dto)
        {
            if (dto.Id != id) return BadRequest();

            Device device = await _unitOfWork.DeviceRepository.FindByIdAsync(id);

            if (device == null) return NotFound();

            device.DeviceType = dto.DeviceType;
            device.Name = dto.Name;
            device.SerialNumber = dto.SerialNumber;

            await _unitOfWork.DeviceRepository.Update(device);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DeviceDto>> DeleteAsync(int id)
        {
            Device device = await _unitOfWork.DeviceRepository.FindByIdAsync(id);

            if (device == null) return NotFound();

            var deleted = _unitOfWork
                .DeviceRepository.Delete(device);
            await _unitOfWork.SaveChangesAsync();

            return new DeviceDto(deleted);
        }
    }
}
