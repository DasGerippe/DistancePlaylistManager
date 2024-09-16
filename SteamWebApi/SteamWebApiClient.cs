using SteamWebApi.ApiInterfaces;

namespace SteamWebApi
{
    public class SteamWebApiClient
    {
        private readonly Dictionary<Type, ISteamWebApiInterface> _ApiInterfaces;

        internal HttpClient HttpClient { get; }

        public SteamWebApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;

            _ApiInterfaces = new Dictionary<Type, ISteamWebApiInterface>();
            AddApiInterface(new SteamRemoteStorage(this));
        }

        public T? Get<T>() where T : class, ISteamWebApiInterface
        {
            _ApiInterfaces.TryGetValue(typeof(T), out ISteamWebApiInterface? apiInterface);
            return apiInterface as T;
        }

        public void AddApiInterface(ISteamWebApiInterface apiInterface)
        {
            Type apiInterfaceType = apiInterface.GetType();
            if (!_ApiInterfaces.TryAdd(apiInterfaceType, apiInterface))
                throw new ArgumentException(
                    $"Steam web API interface of type {apiInterfaceType} is already registered!",
                    nameof(apiInterface));
        }
    }
}
