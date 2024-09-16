using System.Runtime.CompilerServices;

namespace SteamWebApi.ApiInterfaces
{
    public abstract class SteamWebApiInterfaceBase : ISteamWebApiInterface
    {
        private const string SteamWebApiUrl = "https://api.steampowered.com";

        public SteamWebApiClient SteamWebApiClient { get; }

        public abstract string InterfaceName { get; }

        protected SteamWebApiInterfaceBase(SteamWebApiClient steamWebApiClient)
        {
            SteamWebApiClient = steamWebApiClient ?? throw new ArgumentNullException(nameof(steamWebApiClient));
        }

        protected string GetInterfaceMethodUri([CallerMemberName] string? methodName = null, int version = 1)
        {
            return $"{SteamWebApiUrl}/{InterfaceName}/{methodName}/v{version}/";
        }
    }
}
