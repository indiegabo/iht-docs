using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieGabo.CharacterController2D.Data
{
    public class SlopeData
    {
        public bool onSlope;
        public bool higherThanMax;
        public Vector2 normalPerpendicular;
        public bool ascending;
        public bool descending;
        public bool exitingFromAbove;
        public bool exitingFromBelow;
    }
}
