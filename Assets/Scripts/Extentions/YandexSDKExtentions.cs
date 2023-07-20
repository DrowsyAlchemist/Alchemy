using Agava.YandexGames;

public static class YandexSDKExtentions
{
    public static string GetCurrentLang(this YandexGamesEnvironment environment)
    {
        return environment.i18n.lang;
    }
}
