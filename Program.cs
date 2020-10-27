using System;

namespace NINvalidator //NIN: National Identification Number (approx. "personnummer")
{
    class Program
    {
        //Turns out the current year is used in a couple of places.
        //I don't think I'm allowed to use any Date methods, so I just hardcoded it.
        const int CURRENT_YEAR = 2020;

        static bool IsLeapYear(int year)
        {
            if (year % 400 == 0)
            {
                //Years divisible by 400 are leap years.
                return true;
            }
            else if (year % 100 == 0)
            {
                //Years divisible by 100 but not 400 are *not* leap years.
                return false;
            }
            else if (year % 4 == 0)
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
            int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (IsLeapYear(year))
            {
                daysInMonth[1] = 29; //In leap years, february has 29 days.
            }
            /*The parameter "month" is expected to be >=1, so we subtract 1
             * to correct the indexing of the array.*/
            return (day >= 1 && day <= daysInMonth[month - 1]);
        }

        static bool IsValidBirthMonth(int month)
        {
            return (month >= 1 && month <= 12);
        }

        static bool IsValidBirthYear(int year)
        {
            return (year >= 1753 && year <= CURRENT_YEAR);
        }


        static void ValidateNIN(string NIN)
        {
            switch (NIN.Length)
            {
                case 11: //10 digits plus a divider, in YYMMDD-nnnc format.
                    //Get the divider character from the input string.
                    //Its correct position is known from the format, so I've hardcoded it.
                    int dividerPos = NIN.Length - 5;
                    char divider = NIN[dividerPos];

                    if (!(divider == '-' || divider == '+'))
                    {
                        throw new FormatException("Incorrect format: User entered " +
                            "10-digit NIN that contained unsupported divider character "
                            + divider);
                    }
                    int twoDigitBirthYear = int.Parse(NIN.Substring(0, 2)); //YY
                    int age = (CURRENT_YEAR - twoDigitBirthYear) % 100;

                    if (divider == '+')
                    {
                        //Divider '+' is used for people over 100.
                        age += 100;
                    }
                    //Preprocessing to convert the NIN into the 12-digit format
                    NIN = NIN.Remove(dividerPos, 1); //Remove the divider character
                    NIN = (CURRENT_YEAR - age) + NIN.Substring(2); //Replace YY with YYYY.
                    goto case 12; //Fallthrough and let the 12-digit case handle the heavy lifting.
                case 12: //12 digits in YYYYMMDDnnnc format
                    int year = int.Parse(NIN.Substring(0, 4)); //YYYY
                    int month = int.Parse(NIN.Substring(4, 2)); //MM
                    int day = int.Parse(NIN.Substring(6, 2)); //DD
                    int ID = int.Parse(NIN.Substring(8, 3)); //nnn
                    int checksum = int.Parse(NIN.Substring(11)); //c
                    string legalGender = LegalGenderFromID(ID);

                    if (
                        IsValidBirthYear(year) &&
                        IsValidBirthMonth(month) &&
                        IsValidBirthDay(year, month, day) &&
                        LuhnChecksum(NIN.Substring(2)) == checksum
                        )
                    {
                        Console.WriteLine(NIN + " Is a valid NIN. ✔");
                        Console.WriteLine("Legally, the owner of this NIN is considered " + legalGender);
                    }
                    else
                    {
                        Console.WriteLine(NIN + " Is an INVALID NIN. 🚫");
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
            for (int n = 0; n < (NIN.Length - 1); ++n)
            {
                //NIN[n] is a char representing a digit. Subtract '0' to get its value.
                int currentDigit = NIN[n] - '0';
                currentDigit *= (2 - (n % 2)); //rhs alternates between 2 and 1.
                //currentDigit now has at most 2 digits. Add its sum-of-digits to the total.
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
                    Console.Write("Enter a National Identification Number (personnummer)" +
                        "or enter \"quit\" to quit: ");
                    string input = Console.ReadLine();
                    if (input == "quit")
                    {
                        running = false;
                    }
                    else
                    {
                        ValidateNIN(input);
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Supported formats: " +
                        "YYMMDD-nnnc YYMMDD+nnnc YYYYMMDDnnnc");
                }
            } while (running);
        }
    }
}
