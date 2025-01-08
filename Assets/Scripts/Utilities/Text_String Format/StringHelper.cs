using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace util
{
    // A helper class for various string related functions.
    public class StringHelper : MonoBehaviour
    {
        // Gets the number of instances of a substring.
        public static int GetSubstringCount(string str, string substr)
        {
            // First check if the substring exists in the first place.
            if(str.Contains(substr))
            {
                // THe index of the current substring instance.
                int index = 0;

                // The number of instances.
                int instances = 0;

                // While the index is valid.
                while(index >= 0 && index < str.Length)
                {
                    // Get the index
                    index = str.IndexOf(substr, index);

                    // If an instance was found.
                    if(index != -1)
                    {
                        // Increase the number of instances.
                        instances++;

                        // Shift the index.
                        index += substr.Length;
                    }
                    else // No instance found.
                    {
                        break;
                    }
                }

                // Return the instances count.
                return instances;

            }
            else
            {
                return 0;
            }
        }

    }
}