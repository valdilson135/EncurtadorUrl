using AutoMapper;
using EncurtadorUrl.Dtos;
using EncurtadorUrl.Interfaces;
using EncurtadorUrl.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;

namespace EncurtadorUrl.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class UrlController : MainController
    {
        private readonly IUrlService _urlService;
        private readonly IMapper _mapper;
        private readonly IUrlShortService _urlShortService;
        public UrlController(INotificador notificador, IUrlService urlService,
            IMapper mapper, IUrlShortService urlShortService) : base(notificador)
        {
            _urlService = urlService;
            _mapper = mapper;
            _urlShortService = urlShortService;
        }

        [HttpGet]
        public async Task<IEnumerable<UrlDto>> GetAllUrls()
        {
            return _mapper.Map<IEnumerable<UrlDto>>(await _urlService.GetAllUrls());
        }

        [HttpGet]
        [Route("GetUrlById/{id}")]
        public async Task<ActionResult<UrlDto>> GetUrlById(int id)
        {
            var url = await _urlService.GetUrlById(id);
            if (url == null) return NotFound("Url não encontrada.");        

            return CustomResponse(url);
        }       

        [HttpPost]
        public async Task<ActionResult<UrlDto>> CreateUrl([FromBody] UrlCreateDto url)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!Uri.TryCreate(url.Url, UriKind.Absolute, out var urlValida))
            {
                return CustomResponse("Url inválida.");
            }
            
            var urlNew = _mapper.Map<UrlModel>(url);
            urlNew.ShortUrl = _urlShortService.SetUrlShort(url);

            await _urlService.CreateUrl(urlNew);

            return CustomResponse(urlNew);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UrlDto>> UpdateUrl(int id, [FromBody] UrlUpdateDto url)
        {
            if (id != url.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(url);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _urlService.UpdateUrl(_mapper.Map<UrlModel>(url));

            return CustomResponse(url);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<UrlDto>> DeleteUrl(int id)
        {
            var url = await _urlService.GetUrlById(id);

            if (url == null) return NotFound();

            await _urlService.DeleteUrl(url);

            return CustomResponse(url);
        }

        [HttpGet]
        [Route("ValidateShortUrl")]        
        public async Task<ActionResult<UrlDto>> ValidateShortUrl([FromQuery] string ShortUrl)
        {
            var urlShort = new UrlReadDto();
            urlShort.ShortUrl = ShortUrl;

            var url = await _urlService.GetUrlByShortUrl(_mapper.Map<UrlModel>(urlShort));

            if (url == null) return NotFound("Urlencurtada não encontrada!");
            
            url.Hits = url.Hits + 1;

            await _urlService.UpdateUrl(url);
            return CustomResponse(_mapper.Map<UrlDto>(url));
        }        

        [HttpPost]
        [Route("ProcessFile")]
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
