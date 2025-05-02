using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TradingService.Shared.Helpers;

/// <summary>
/// Provides helper methods for asynchronous JSON serialization and deserialization.
/// </summary>
public static class JsonHelper
{
    private static readonly JsonSerializerOptions defaultOptions = new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    /// <summary>
    /// Asynchronously serializes an object to a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> that contains the JSON string representation of the object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="obj"/> is <see langword="null"/>.</exception>
    /// <exception cref="NotSupportedException">Thrown when the type of <paramref name="obj"/> is not supported for serialization.</exception>
    public static async Task<string> SerializeAsync<T>(
        T obj,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(obj);

        using var stream = new MemoryStream();

        await JsonSerializer.SerializeAsync(stream, obj, defaultOptions, cancellationToken)
            .ConfigureAwait(false);

        stream.Position = 0;
        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously deserializes a JSON string to an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> that contains the deserialized object of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="json"/> is empty or whitespace.</exception>
    /// <exception cref="JsonException">Thrown when the JSON string is not valid or cannot be deserialized to <typeparamref name="T"/>.</exception>
    public static async Task<T?> DeserializeAsync<T>(
        string? json,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        byte[] bytes = Encoding.UTF8.GetBytes(json);
        using var stream = new MemoryStream(bytes);

        return await JsonSerializer.DeserializeAsync<T>(stream, defaultOptions, cancellationToken)
            .ConfigureAwait(false);
    }
}
