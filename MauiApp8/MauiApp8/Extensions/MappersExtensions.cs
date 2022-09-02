using MauiApp8.Mappers;

namespace MauiApp8.Extensions;
public static class MappersExtensions
{
    public static MauiAppBuilder UseMappers(this MauiAppBuilder builder)
    {
        EntryHandlerMapper.AppendToMpping();

        return builder;
    }
}
