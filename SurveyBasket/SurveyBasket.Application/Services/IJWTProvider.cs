using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBasket.Application.Services;
public interface IJWTProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user);
}
