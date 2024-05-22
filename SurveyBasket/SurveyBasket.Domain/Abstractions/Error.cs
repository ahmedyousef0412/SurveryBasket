using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBasket.Domain.Abstractions;
public record Error(string Code , string Description ,int? StautsCode)
{
    public static readonly Error None = new (string.Empty,string.Empty, null);
}


//Above Code  equal to the belwo code

//internal class Error
//{
//    private readonly string _code;
//    private readonly string _description;
//    public Error(string Code ,string Description)
//    {
//        _code = Code;
//        _description = Description;
//    }
//}