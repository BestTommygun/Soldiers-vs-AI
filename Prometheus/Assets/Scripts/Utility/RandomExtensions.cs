using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomExtensions
{
    public static class RandomExtensions
    {
        public static bool Bool(this Random random)
        {
            bool returnVal = false;
            if (Random.value > 0.5f) returnVal = true;
            return returnVal;
        }
    }
}
