using GlobusAssessment.Application.DTOs;
using GlobusAssessment.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace GLobusAssessment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUserService _userService;

        public CustomerController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// This Endpoint Onboards a new customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer([FromQuery] AddCustomerDto model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            var response = await _userService.RegisterCustomer(model);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// This Endpoint confirms a customer after registration. (default OTP = 546723)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpPatch("{customerId}")]
        public async Task<IActionResult> ConfirmCustomer([FromBody] ConfirmPhoneNumberDto model, string customerId)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            var reaponse = await _userService.ConfirmCustomer(model.OTP, customerId);

            return StatusCode(Response.StatusCode, reaponse);
        }

        /// <summary>
        /// This endpoint returns all registered customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var response = await _userService.GetCustomersAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
