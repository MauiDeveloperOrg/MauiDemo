using MauiApp9.Mappers;

namespace MauiApp9.Extensions;
public static class MappersExtensions
{
    public static MauiAppBuilder UseMappers(this MauiAppBuilder builder)
    {
        EntryHandlerMapper.AppendToMpping();

        return builder;
    }
}
