using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBasket.Contracts.Responses;
public record PollResponse(int Id, string Title, string Notes);
