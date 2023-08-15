using EncurtadorUrl.Dtos;
using EncurtadorUrl.Interfaces;
using EncurtadorUrl.Models;
using EncurtadorUrl.Models.Validations;
using NanoidDotNet;
using Newtonsoft.Json;
using System;

namespace EncurtadorUrl.Data.Services
{
    public class UrlService : BaseService, IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private IRabbitMqClient _rabbitMqClient;
        public UrlService(IUrlRepository urlRepository, IRabbitMqClient rabbitMqClient,
                              INotificador notificador) : base(notificador)
        {
            _urlRepository = urlRepository;
            _rabbitMqClient = rabbitMqClient;
        }

        public async Task<bool> CreateUrl(UrlModel url)
        {
            if (!ExecutarValidacao(new UrlValidation(), url)) return false;

            var ret = await _urlRepository.GetUrlByUrl(url);
            if (ret != null)
            {
                Notificar("Já existe uma Url com este endereço infomado.");
                return false;
            }
            //Adiciona a Url Encurtada            
            await _urlRepository.CreateUrl(url);

            _rabbitMqClient.PublicUrl(url);

            return true;
        }

        public async Task<bool> UpdateUrl(UrlModel url)
        {
            if (!ExecutarValidacao(new UrlValidation(), url)) return false;
            UrlModel urlUpd = await _urlRepository.GetUrlById(url.Id);

            if (urlUpd.Id <= 0)
            {
                Notificar("Id informado não existe.");
                return false;
            }

            urlUpd.Url = url.Url;
            await _urlRepository.UpdateUrl(urlUpd);

            return true;
        }

        public async Task<IEnumerable<UrlModel>> GetAllUrls()
        {
            return await _urlRepository.GetAllUrls();
        }

        public async Task<UrlModel> GetUrlById(int id)
        {
            return await _urlRepository.GetUrlById(id);
        }

        public async Task<UrlModel> GetUrlByUrl(UrlModel url)
        {
            return await _urlRepository.GetUrlByUrl(url);
        }

        public async Task<UrlModel> GetUrlByShortUrl(UrlModel url)
        {
            return await _urlRepository.GetUrlByShortUrl(url);
        }
        public async Task DeleteUrl(UrlModel url)
        {
            await _urlRepository.DeleteUrl(url);
        }

        public async Task<bool> ProcessFile(FileUploadDto model)
        {
            try
            {               
                using (var reader = new StreamReader(model.File.OpenReadStream()))
                {
                    var fileContent = await reader.ReadToEndAsync();

                    List<UrlModel> listUrl = JsonConvert.DeserializeObject<List<UrlModel>>(fileContent);

                    foreach (var item in listUrl)
                    {
                        var existUrl = await _urlRepository.GetUrlByShortUrl(item);
                        if (existUrl == null)
                        {
                            item.Id = 0;
                            await _urlRepository.CreateUrl(item);
                        }
                        else { Notificar($"Url existenta na base de Dados. UrlPrincipal({item.Url}), UrlEncurtada({item.ShortUrl}) "); }
                    }
                }
            }
            catch (Exception ex)
            {
                Notificar($"Erro ao tentar processar arquivo de dados. ex.Message({ ex.Message}), ex.StackTrace({ex.StackTrace}) ");
                return false;
            }
            return true;
        }
    }
}
