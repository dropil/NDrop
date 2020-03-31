using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NDrop.Utils
{    
    public class JSON
    {
        /// <summary>
        /// Sorts a JObject by key name; also traverses any nested JObjects and JArrays and sorts those objects
        /// </summary>
        /// <param name="jObj"></param>
        /// <returns></returns>
        public static JObject Sort(JObject jObj)
        {
            var props = jObj.Properties().ToList();

            // remove all properties since we will be adding them back in the correct order below
            foreach (var prop in props)
                prop.Remove();

            // loop through all properties sorted by key name
            foreach (var prop in props.OrderBy(p => p.Name))
            {
                jObj.Add(prop);

                if (prop.Value is JObject) // checking if nested item is also an object
                    Sort((JObject)prop.Value); // sort nested object
                else if (prop.Value is JArray) // checking if nested object is an array
                    foreach (var item in prop.Value) // loop through each item in array
                        if (item is JObject) // checking if array item is an object
                            Sort((JObject)item); // sort object
            }

            return jObj;
        }
    }    
}
