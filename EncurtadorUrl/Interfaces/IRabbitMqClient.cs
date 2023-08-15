using EncurtadorUrl.Models;

namespace EncurtadorUrl.Interfaces
{
    public interface IRabbitMqClient
    {
        void PublicUrl(UrlModel url);
    }
}
