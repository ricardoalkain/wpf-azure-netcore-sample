using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using TTMS.Common.DTO;
using TTMS.Common.DTO.Extensions;
using TTMS.Common.Enums;
using TTMS.Common.Models;
using TTMS.Web.Api.Core.Services;

namespace TTMS.Web.Api.Controllers
{
    [Route("api/v0.1/travelers")]
    public class TravelerController : ControllerBase
    {
        private readonly ITravelerDbService service;
        private readonly ILogger<TravelerController> logger;

        public TravelerController(ILogger<TravelerController> logger, ITravelerDbService travelerService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.service = travelerService ?? throw new ArgumentNullException(nameof(travelerService));
        }

        /// <summary>
        /// Returns a list of travelers.
        /// </summary>
        /// <returns>List of travelers registered in the system</returns>
        //[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<TravelerResponse>), Description = "List of travelers in the system")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Traveler> travelers = await service.GetAllAsync().ConfigureAwait(false);
            return Ok(travelers.CreateResponse());
        }

        /// <summary>
        /// Returns information about a traveler
        /// </summary>
        /// <param name="id">Traveler's ID</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(TravelerResponse), Description = "Information about the requested traveler")]
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var traveler = await service.GetByIdAsync(id).ConfigureAwait(false);
            return traveler == null
                    ? (IActionResult)NotFound()
                    : Ok(traveler.CreateResponse());
        }

        /// <summary>
        /// Returns a list of traveler of a specific type
        /// </summary>
        /// <param name="type">Traveler's type</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(TravelerResponse), Description = "Information about the requested traveler")]
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetBytype(TravelerType type)
        {
            var traveler = await service.GetByTypeAsync(type).ConfigureAwait(false);
            return Ok(traveler.CreateResponse());
        }

        /// <summary>
        /// Registers a new traveler
        /// </summary>
        /// <param name="traveler">Information about the new traveler</param>
        /// <returns>Traveler created</returns>
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(TravelerResponse), Description = "Registers a new traveler")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TravelerRequest traveler)
        {
            var newTraveler = await service.CreateAsync(traveler.ToModel()).ConfigureAwait(false);
            var response = newTraveler.CreateResponse();

            return Created(Url.Link("DefaultApi", new { controller = "Traveler", id = response.Id }), response);
        }

        /// <summary>
        /// Upates information about a traveler
        /// </summary>
        /// <param name="id">ID identifying the traveler to be updated</param>
        /// <param name="traveler">Traveler's data</param>
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Updates a traveler's data")]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]TravelerRequest traveler)
        {
            if (id != traveler.Id)
            {
                var msg = "It's not allowed to change entity ID.";
                logger.LogWarning("BAD REQUEST: {msg} => {@Request}", msg, Request);
                return BadRequest(msg);
            }

            await service.UpdateAsync(traveler.ToModel()).ConfigureAwait(false);

            return Ok();
        }

        /// <summary>
        /// Removes a traveler from the database
        /// </summary>
        /// <param name="id">ID of the traveler to be removed</param>
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Removes a traveler")]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await service.DeleteAsync(id).ConfigureAwait(false);

            return Ok();
        }
    }
}
