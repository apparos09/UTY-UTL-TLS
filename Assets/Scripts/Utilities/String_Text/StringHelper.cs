using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace util
{
    // A helper class for various string related functions.
    public class StringHelper : MonoBehaviour
    {
        // Returns the number of English alphabet letters.
        public static int GetNumberOfEnglishAlphabetLetters()
        {
            return 26;
        }

        // Returns the English alphabet number of the provided letter.
        // -1 is returned if the char isn't a recognized lettter.
        public static int GetEnglishAlphabetLetterNumber(char letter)
        {
            // The letters of the alphabet.
            // An extra space is added in slot (0) to make the numbers match up.
            List<char> letters = new List<char>()
            {
                ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 
                'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 
                'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };

            // THe uppercase verison of the character.
            // This is used to search the list of letters.
            char upperChar = char.ToUpper(letter);

            // The number of the letter.
            int letterNumber;

            // Checks if the character is in the letter list.
            if(letters.Contains(upperChar))
            {
                // Gets the index of the letter and uses it as the number.
                // The blank slot at index (0) allows the index number to match up to the alphabet number.
                letterNumber = letters.IndexOf(upperChar);
            }
            else
            {
                // the letter isn't in the list, so put -1.
                letterNumber = -1;
            }

            // Returns the letter number.
            return letterNumber;
        }

        // Gets the alphabet letter that the number corresponds to.
        // Returns a space character (' ') if it's an invalid number.
        public static char GetEnglishAlphabetLetterByNumber(int number)
        {
            // The letters of the alphabet.
            // An extra space is added in slot (0) to make the numbers match up.
            List<char> letters = new List<char>()
            {
                ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
                'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
                'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };

            // The result.
            char letter;

            // If the number is greater than 0 and less than the letter count, get the number.
            // Index 0 is ignored since it's a space character and is not actually part of the alphabet.
            // The space is just there to make the letters match up with their numbers.
            if(number > 0 && number < letters.Count)
            {
                // Gets the letter.
                letter = letters[number];
            }
            else
            {
                // Wasn't a valid number, so return a space.
                letter = ' ';
            }

            return letter;
        }

        // Gets the number of instances of a substring.
        public static int GetSubstringCount(string str, string substr)
        {
            // If the substring is blank.
            if(substr == string.Empty)
            {
                // If the string is also empty, count this as 1 instance.
                if(str == string.Empty)
                {
                    return 1;
                }
                // The string isn't empty, so return a instance count of 0.
                else
                {
                    return 0;
                }
            }

            // If the substring is not in the string at all, return 0.
            if(!str.Contains(substr))
            {
                return 0;
            }

            // OLD - checks each index.
            // // First check if the substring exists in the first place.
            // if(str.Contains(substr))
            // {
            //     // THe index of the current substring instance.
            //     int index = 0;
            // 
            //     // The number of instances.
            //     int instances = 0;
            // 
            //     // While the index is valid.
            //     while(index >= 0 && index < str.Length)
            //     {
            //         // Get the index
            //         index = str.IndexOf(substr, index);
            // 
            //         // If an instance was found.
            //         if(index != -1)
            //         {
            //             // Increase the number of instances.
            //             instances++;
            // 
            //             // Shift the index.
            //             index += substr.Length;
            //         }
            //         else // No instance found.
            //         {
            //             break;
            //         }
            //     }
            // 
            //     // Return the instances count.
            //     return instances;
            // 
            // }
            // else
            // {
            //     return 0;
            // }

            // V2 - uses replace function to compare strings
            
            // Makes a temporary string with all instances of the substring removed.
            string tempStr = str.Replace(substr, "");

            // Gets the difference in length between the two strings.
            int lenDiff = str.Length - tempStr.Length;

            // Calculates the number of instances.
            // Divide by 0 should not be possibly at this stage.
            int instances = lenDiff / substr.Length;

            // Returns the instance count.
            return instances;
        }

    }
}