using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBasket.Infrastruction.EntitiesConfiguration;
internal sealed class JwtConfiguration
{
    public static string SectionName = "JWT";

    [Required]
    public string Key { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Range(10,60)]
    public int ExpireInMinute { get; init; }
    

}
