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

        public UrlController(INotificador notificador, IUrlService urlService,
            IMapper mapper ) : base(notificador)
        {
            _urlService = urlService;
            _mapper = mapper;            
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

            var retUrl =  await _urlService.CreateUrl(url);

            return CustomResponse(retUrl);
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

            var retUrl = await _urlService.UpdateUrl(url);

            return CustomResponse(retUrl);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<UrlDto>> DeleteUrl(int id)
        {
            var retUrl = await _urlService.DeleteUrl(id);
            return CustomResponse(retUrl);
        }

        [HttpGet]
        [Route("ValidateShortUrl")]        
        public async Task<ActionResult<UrlDto>> ValidateShortUrl([FromQuery] string shortUrl)
        {
            var urlShortGet = new UrlReadDto();
            urlShortGet.ShortUrl = shortUrl;

            var retUrl = await _urlService.ValidateUrl(urlShortGet);

            return CustomResponse(retUrl);
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
