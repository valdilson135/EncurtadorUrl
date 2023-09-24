using AutoMapper;
using EncurtadorUrl.Dtos;
using EncurtadorUrl.Interfaces;
using EncurtadorUrl.Models;
using EncurtadorUrl.Models.Validations;
using Newtonsoft.Json;
using System;

namespace EncurtadorUrl.Data.Services
{
    public class UrlService : BaseService, IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private IRabbitMqClient _rabbitMqClient;
        private readonly IMapper _map;
        private readonly IUrlShortService _urlShortService;

        public UrlService(IUrlRepository urlRepository, IRabbitMqClient rabbitMqClient,
            IUrlShortService urlShortService, IMapper map, INotificador notificador) : base(notificador)
        {
            _urlRepository = urlRepository;
            _rabbitMqClient = rabbitMqClient;
            _map = map;
            _urlShortService = urlShortService;
        }

        public async Task<bool> CreateUrl(UrlCreateDto url)
        {
            if (!ExecutarValidacao(new UrlValidation(), url)) return false;

            var newUrl = new UrlModel(url.Url);

            var urlShort = _urlShortService.SetUrlShort();
            newUrl.SetShortUrl(urlShort);

            var existUrl = await _urlRepository.GetUrlByUrl(newUrl);
            if (existUrl != null)
            {
                Notificar("Já existe uma Url com este endereço infomado.");
                return false;
            }
            //Adiciona a Url Encurtada            
            var urlReturn = await _urlRepository.CreateUrl(newUrl);

            _rabbitMqClient.PublicUrl(urlReturn);

            return true;
        }

        public async Task<bool> UpdateUrl(UrlUpdateDto url)
        {
            if (!ExecutarValidacao(new UrlUpValidation(), url)) return false;

            var urlUpd = await _urlRepository.GetUrlById(url.Id);

            if (urlUpd.Id <= 0)
            {
                Notificar("Id informado não existe.");
                return false;
            }

            urlUpd.Url = url.Url;
            await _urlRepository.UpdateUrl(urlUpd);

            return true;
        }

        public async Task<UrlDto> ValidateUrl(UrlReadDto shortUrl)
        {
            var urlValidate = new UrlModel(string.Empty);
            urlValidate.SetShortUrl(shortUrl.ShortUrl);

            var urlGet = await _urlRepository.GetUrlByShortUrl(urlValidate);
            if (urlGet == null)
            {
                Notificar("Id informado não existe.");
                return _map.Map<UrlDto>(urlGet);
            }

            urlGet.SetHints(1);

            var returnUrl = await _urlRepository.UpdateUrl(urlGet);

            return _map.Map<UrlDto>(returnUrl);
        }

        public async Task<IEnumerable<UrlDto>> GetAllUrls()
        {
            var urlReturn = await _urlRepository.GetAllUrls();

            return _map.Map<IEnumerable<UrlDto>>(urlReturn);
        }

        public async Task<UrlDto> GetUrlById(int id)
        {
            var urlReturn = await _urlRepository.GetUrlById(id);

            return _map.Map<UrlDto>(urlReturn);
        }       
      
        public async Task<UrlDto> DeleteUrl(int id)
        {
            var urlDelet = await _urlRepository.GetUrlById(id);
            if (urlDelet == null)
            {
                Notificar("Id informado não existe.");
                return _map.Map<UrlDto>(urlDelet);
            }

            var urlReturn = await _urlRepository.DeleteUrl(urlDelet);

            return _map.Map<UrlDto>(urlReturn);
        }

        public async Task<bool> ProcessFile(FileUploadDto model)
        {
            try
            {
                using (var reader = new StreamReader(model.File.OpenReadStream()))
                {
                    var fileContent = await reader.ReadToEndAsync();

                    List<UrlDto> listUrl = JsonConvert.DeserializeObject<List<UrlDto>>(fileContent);

                    foreach (var item in listUrl)
                    {
                        var existUrl = await _urlRepository.GetUrlByShortUrl(_map.Map<UrlModel>(item));
                        if (existUrl == null)
                        {
                            var newUrl = new UrlModel(item.Url);
                            newUrl.SetShortUrl(item.ShortUrl);
                            newUrl.SetHints(item.Hits);
                            await _urlRepository.CreateUrl(newUrl);
                        }
                        else { Notificar($"Url existenta na base de Dados. UrlPrincipal({item.Url}), UrlEncurtada({item.ShortUrl}) "); }
                    }
                }
            }
            catch (Exception ex)
            {
                Notificar($"Erro ao tentar processar arquivo de dados. ex.Message({ex.Message}), ex.StackTrace({ex.StackTrace}) ");
                return false;
            }
            return true;
        }

        public async Task<List<UrlModel>> ProcessFile(string filePath)
        {
            var listUrl = new List<UrlModel>();
            try
            {
                var contentFile = File.ReadAllText(filePath);
                var dadosFile = JsonConvert.DeserializeObject<List<UrlDto>>(contentFile);
                
                foreach (var item in dadosFile)
                {
                    var existUrl = await _urlRepository.GetUrlByShortUrl(_map.Map<UrlModel>(item));
                    if (existUrl == null)
                    {
                        var newUrl = new UrlModel(item.Url);
                        newUrl.SetShortUrl(item.ShortUrl);
                        newUrl.SetHints(item.Hits);
                        listUrl.Add(newUrl);    
                    }
                    else { Notificar($"Url existenta na base de Dados. UrlPrincipal({item.Url}), UrlEncurtada({item.ShortUrl}) "); }
                }
            }
            catch (Exception ex)
            {
                Notificar($"Erro ao tentar processar arquivo de dados. ex.Message({ex.Message}), ex.StackTrace({ex.StackTrace}) ");
                return listUrl;
            }
            return listUrl;
        }
    }
}
