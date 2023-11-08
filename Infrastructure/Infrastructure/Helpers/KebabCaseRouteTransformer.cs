using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace Infrastructure.Helpers;

public sealed class KebabCaseRouteTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object value) => value != null
        ? Regex.Replace(value.ToString() ?? string.Empty, "([a-z])([A-Z])", "$1-$2").ToLower()
        : null;
}