using EncurtadorUrl.Dtos;
using EncurtadorUrl.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EncurtadorUrl.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/urls")]
    public class UrlController : MainController
    {
        private readonly IUrlService _urlService;

        public UrlController(INotificador notificador, IUrlService urlService) : base(notificador)
        {
            _urlService = urlService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IEnumerable<UrlDto>> GetAllUrls()
        {
            return await _urlService.GetAllUrls();
        }

        [AllowAnonymous]
        //[EnableCors("Production")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UrlDto>> GetUrlById(int id)
        {
            var url = await _urlService.GetUrlById(id);
            if (url == null) return NotFound("Url não encontrada.");        

            return CustomResponse(url);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> CreateUrl(UrlCreateDto url)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!Uri.TryCreate(url.Url, UriKind.Absolute, out var urlValida))
            {
                return CustomResponse("Url inválida.");
            }           

            var retUrl =  await _urlService.CreateUrl(url);

            return CustomResponse(retUrl);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateUrl(int id, UrlUpdateDto url)
        {
            if (id != url.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(url);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var retUrl = await _urlService.UpdateUrl(url);

            return CustomResponse(retUrl);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UrlDto>> DeleteUrl(int id)
        {
            var retUrl = await _urlService.DeleteUrl(id);
            return CustomResponse(retUrl);
        }

        [AllowAnonymous]
        //[EnableCors("Production")]
        [HttpGet("{shortUrl:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UrlDto>> ValidateShortUrl(string shortUrl)
        {
            var urlShortGet = new UrlReadDto();
            urlShortGet.ShortUrl = shortUrl;

            var retUrl = await _urlService.ValidateUrl(urlShortGet);

            return CustomResponse(retUrl);
        }        

        [HttpPost]
        [Route("Process")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ProcessFile([FromForm] FileUploadDto model)
        {
            if (model == null || model.File.Length == 0 )
            {
                return BadRequest("Não há arquivo para processamento.");
            }

            var ret =  await _urlService.ProcessFile(model);
           
            return CustomResponse(ret);
        }
    }
}
