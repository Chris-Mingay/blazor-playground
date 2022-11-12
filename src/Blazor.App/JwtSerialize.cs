using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Blazor.App;

public class JwtSerialize
{
    public static ClaimsPrincipal Deserialize(string jwtToken)
    {
        var segments = jwtToken.Split('.');

        if (segments.Length != 3)
        {
            throw new Exception("Invalid JWT");
        }
        
        var dataSegment = Encoding.UTF8.GetString(FromUrlBase64(segments[1]));
        var data = JsonSerializer.Deserialize<JsonObject>(dataSegment);

        var claims = new List<Claim>();
        foreach (var entry in data)
        {
            if (entry.Value is null) continue;

            var key = entry.Key;
            var value = entry.Value.ToString();
            
            if (value.Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(value);
                foreach (var parsedRole in parsedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }
            }
            else
            {
                claims.Add(new Claim(key, value));
            }
        }

        var claimIdentity = new ClaimsIdentity(claims, "jwt");
        var principal = new ClaimsPrincipal(new[] { claimIdentity });

        return principal;
    }

    private static Claim JwtNodeToClaim(string key, JsonNode node)
    {
        

        if (node is JsonArray)
        {
            return new Claim(key, JsonSerializer.Serialize(node.AsArray()), JsonClaimValueTypes.JsonArray);
        }
        
        var jsonValue = node.AsValue();
        
        if (jsonValue.TryGetValue<string>(out var str))
        {
            return new Claim(key, str, ClaimValueTypes.String);
        }
        
        if (jsonValue.TryGetValue<double>(out var num))
        {
            return new Claim(key, num.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Double);
        }
        
        if (jsonValue.TryGetValue<string[]>(out var arr))
        {
            return new Claim(key, JsonSerializer.Serialize(arr), JsonClaimValueTypes.JsonArray);
        }
        
        throw new Exception("Unsupported JWT claim type");
    }

    private static byte[] FromUrlBase64(string jwtSegment)
    {
        string fixedBase64 = jwtSegment
            .Replace('-', '+')
            .Replace('_', '/');

        switch (jwtSegment.Length % 4)
        {
            case 2: fixedBase64 += "=="; break;
            case 3: fixedBase64 += "="; break;
            default: throw new Exception("Illegal base64url string!");
        }

        return Convert.FromBase64String(fixedBase64);
    }
}