using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using TTMS.Common.DTO;
using TTMS.Common.Enums;
using TTMS.Common.Models;
using TTMS.Web.Api.Services;

namespace TTMS.Web.Api.Controllers
{
    public class TravelerController : ApiController
    {
        private readonly ITravelerApiService dataService;

        public TravelerController(ITravelerApiService travelerService)
        {
            dataService = travelerService;
        }

        /// <summary>
        /// Returns a list of travelers.
        /// </summary>
        /// <param name="filterByStatus">Filter results by this status</param>
        /// <param name="loadPictures">Load traveler's picture data</param>
        /// <returns>List of travelers registered in the system</returns>
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<TravelerResponse>), Description = "List of travelers in the system")]
        public async Task<IHttpActionResult> Get(
            [FromUri]TravelerStatus? filterByStatus = null,
            [FromUri]bool loadPictures = false)
        {
            var travelers = await dataService.GetAllAsync(filterByStatus, loadPictures).ConfigureAwait(false);
            return Ok(travelers.CreateResponse());
        }

        /// <summary>
        /// Returns information about a
        /// </summary>
        /// <param name="id">Traveler's ID</param>
        /// <param name="loadPicture">If "true" returns traveler's picture data</param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TravelerResponse), Description = "Information about the requested traveler")]
        public async Task<IHttpActionResult> Get(Guid id, [FromUri]bool loadPicture = false)
        {
            var traveler = await dataService.GetByIdAsync(id, loadPicture).ConfigureAwait(false);
            return Ok(traveler.CreateResponse());
        }

        /// <summary>
        /// Registers a new traveler
        /// </summary>
        /// <param name="traveler">Information about the new traveler</param>
        /// <returns>Traveler created</returns>
        [SwaggerResponse(HttpStatusCode.Created, Type = typeof(TravelerResponse), Description = "Registers a new traveler")]
        public async Task<IHttpActionResult> Post([FromBody]TravelerRequest traveler)
        {
            if (!traveler.Id.HasValue)
            {
                traveler.Id = Guid.NewGuid();
            }

            var newTraveler = await dataService.CreateAsync(traveler.ToModel()).ConfigureAwait(false);
            var response = newTraveler.CreateResponse();

            return Created(Url.Link("DefaultApi", new { controller = "Traveler", id = response.Id }), response);
        }

        /// <summary>
        /// Upates information about a traveler
        /// </summary>
        /// <param name="id">ID identifying the traveler to be updated</param>
        /// <param name="traveler">Traveler's data</param>
        [SwaggerResponse(HttpStatusCode.OK, Description = "Updates a traveler's data")]
        public async Task<IHttpActionResult> Put(Guid id, [FromBody]TravelerRequest traveler)
        {
            if (id != traveler.Id)
            {
                return BadRequest("It's not allowed to change entity ID.");
            }

            await dataService.UpdateAsync(traveler.ToModel()).ConfigureAwait(false);

            return Ok();
        }

        /// <summary>
        /// Removes a traveler from the database
        /// </summary>
        /// <param name="id">ID of the traveler to be removed</param>
        [SwaggerResponse(HttpStatusCode.OK, Description = "Removes a traveler")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await dataService.DeleteAsync(id).ConfigureAwait(false);

            return Ok();
        }
    }
}
