using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globomantics.Authorization
{
    public class YearOfExperienceRequirement: IAuthorizationRequirement
    {
        public int YearOfExperienceRequired { get; set; }
        public YearOfExperienceRequirement(int yearOfExperienceRequired)
        {
            YearOfExperienceRequired = yearOfExperienceRequired;
        }
    }
}
