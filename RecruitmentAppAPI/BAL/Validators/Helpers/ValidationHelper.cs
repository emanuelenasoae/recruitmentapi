using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class ValidationHelper
{
    public static bool BeAValidName(string name)
    {
        string regex = @"^[A-Za-z ,.'-]+$";
        return Regex.IsMatch(name, regex);
    }

    public static bool BeAValidString(string stringToVerify)
    {
        string regex = @"^[A-Za-z '\-()]+$";
        return Regex.IsMatch(stringToVerify, regex);
    }
}

