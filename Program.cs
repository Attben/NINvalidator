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

        static bool IsValidBirthMonth(int month)
        {
            return (month >= 1 && month <= 12);
        }

        static bool IsValidBirthYear(int year)
        {
            return (year >= 1753 && year <= 2020);
        }

        static void ValidateNIN(string userInput)
        {
            switch (userInput.Length)
            {
                case 11:
                    //Not yet implemented
                    break;
                case 12:

                    break;
                default:
                    Console.WriteLine("Incorrect format: User entered NIN of unsupported length.");
                    break;
            }
        }
        static void Main(string[] args)
        {
            Console.Write("Enter a National Identification Number (personnummer): ");
            ValidateNIN(Console.ReadLine());
        }
    }
}
