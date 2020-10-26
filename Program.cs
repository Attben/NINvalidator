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
            //Bounds are intentionally hardcoded to match the project specification.
            return (year >= 1753 && year <= 2020);
        }


        static void ValidateNIN(string userInput)
        {
            switch (userInput.Length)
            {
                case 11: //10 digits plus a divider, in YYMMDD-nnnc format.
                    //Get the divider character from the input string
                    char divider = userInput[userInput.Length - 5];
                    string prefix;
                    if (divider == '-')
                    {
                        //TODO
                    }
                    break;
                case 12: //12 digits in YYYYMMDDnnnc format
                    int year = int.Parse(userInput.Substring(0,4));
                    int month = int.Parse(userInput.Substring(4, 2));
                    int day = int.Parse(userInput.Substring(6, 2));
                    int ID = int.Parse(userInput.Substring(8, 3));
                    int checksum = int.Parse(userInput.Substring(11));
                    string legalGender = LegalGenderFromID(ID);

                    if (
                        IsValidBirthYear(year) &&
                        IsValidBirthMonth(month) &&
                        IsValidBirthDay(year, month, day) &&
                        LuhnChecksum(userInput.Substring(2)) == checksum 
                        )
                    {
                        Console.WriteLine(userInput + " Is a valid NIN. ✔");
                        Console.WriteLine("Legally, the owner of this NIN is considered " + legalGender);
                    }
                    else
                    {
                        Console.WriteLine(userInput + " Is an INVALID NIN. 🚫");
                    }
                    break;
                default:
                    throw new FormatException("Incorrect format: NIN has unsupported length.");
            }
        }

        static string LegalGenderFromID(int ID)
        {
            return (ID % 2 == 0) ? "Female" : "Male"; //Even ID: Female. Odd ID: Male.
        }

        static int LuhnChecksum(string NIN)
        {
            int sum = 0;
            //Disregard the final digit, since that's where the checksum goes.
            for(int n = 0; n < (NIN.Length - 1); ++n)
            {
                //NIN[n] is a char representing a digit. Subtract '0' to get its value.
                int currentDigit = NIN[n] - '0';
                currentDigit *= (2 - (n % 2)); //rhs alternates between 2 and 1.
                //currentDigit has at most 2 digits. Add its sum-of-digits to the total.
                sum += currentDigit / 10;
                sum += currentDigit % 10;
            }
            return (10 - (sum % 10)) % 10;
        }

        static void Main(string[] args)
        {
            bool running = true;
            do
            {
                try
                {
                    Console.Write("Enter a National Identification Number (personnummer): ");
                    ValidateNIN(Console.ReadLine());
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
