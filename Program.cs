using System;

namespace NINvalidator //NIN: National Identification Number (approx. "personnummer")
{
    class Program
    {
        static bool IsLeapYear(int year)
        {
            if(year % 400 == 0)
            {
                //Years divisible by 400 are leap years.
                return true;
            }
            else if(year % 100 == 0)
            {
                //Years divisible by 100 but not 400 are *not* leap years.
                return false;
            }
            else if(year % 4 == 0)
            {
                /*Years that don't meet the above conditions, but are
                 * divisible by 4, are leap years.*/
                return true;
            }
            else
            {
                //Any other year is not a leap year.
                return false;
            }
        }

        static bool IsValidBirthDay(int year, int month, int day)
        {
            //Number of days in each month
            int[] daysInMonth = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
            if (IsLeapYear(year))
            {
                daysInMonth[1] = 29; //In leap years, february has 29 days.
            }
            /*The parameter "month" is expected to be >=1, so we subtract 1
             * to correct the indexing of the array.*/
            return (day >= 1 && day <= daysInMonth[month-1]);
        }

        static bool IsValidBirthMonth(int month)
        {
            return (month >= 1 && month <= 12);
        }

        static bool IsValidBirthYear(int year)
        {
            return (year >= 1753 && year <= 2020);
        }


        static bool IsValidNIN(string userInput)
        {
            switch (userInput.Length)
            {
                case 11:
                    //Not yet implemented
                    return false;
                case 12: //12 digits in YYYYMMDDnnnc format
                    int year = int.Parse(userInput.Substring(0,4));
                    int month = int.Parse(userInput.Substring(4, 2));
                    int day = int.Parse(userInput.Substring(6, 2));
                    int ID = int.Parse(userInput.Substring(8, 3));
                    int checksum = int.Parse(userInput.Substring(11));

                    return (
                        IsValidBirthYear(year) &&
                        IsValidBirthMonth(month) &&
                        IsValidBirthDay(year, month, day)
                        );
                default:
                    throw new FormatException("Incorrect format: NIN has unsupported length.");
            }
        }
        static void Main(string[] args)
        {
            bool running = true;
            do
            {
                try
                {
                    Console.Write("Enter a National Identification Number (personnummer): ");
                    string input = Console.ReadLine();
                    if (IsValidNIN(input))
                    {
                        Console.WriteLine(input + "Is a valid NIN. ✔");
                    }
                    else
                    {
                        Console.WriteLine(input + "Is an INVALID NIN. 🚫");
                    }
                    running = false;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (running);
        }
    }
}
