using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UmbrellaUtils.Utils
{
    public static class MathUtils
    {
        private static readonly int CharA = Convert.ToInt32('a');
 
        private static readonly Dictionary<int, string> Units = new Dictionary<int, string>
        {
            {0, ""},
            {1, "K"},
            {2, "M"},
            {3, "B"},
            {4, "T"}
        };
        
        public static Vector2 RandomPointOnCircle(float radius)
        {
            return Random.insideUnitCircle.normalized * radius;
        }

        public static Vector3 RandomPointOnSphere(float radius)
        {
            return Random.onUnitSphere * radius;
        }
        
        public static Vector3 QuadraticLerp(Vector3 posA, Vector3 posB, Vector3 posC, float interpolateAmount)
        {
            Vector3 ab = Vector3.Lerp(posA, posB, interpolateAmount);
            Vector3 bc = Vector3.Lerp(posB, posC, interpolateAmount);

            return Vector3.Lerp(ab, bc, interpolateAmount);
        }

        public static Vector3 CubicLerp(Vector3 posA, Vector3 posB, Vector3 posC, Vector3 posD, float interpolateAmount)
        {
            Vector3 ab_bc = QuadraticLerp(posA, posB, posC, interpolateAmount);
            Vector3 bc_cd = QuadraticLerp(posB, posC, posD, interpolateAmount);
            return Vector3.Lerp(ab_bc, bc_cd, interpolateAmount);
        }

        public static string FormatNumber(double value)
        {
            if (value < 1d)
            {
                return "0";
            }
 
            var n = (int) Math.Log(value, 1000);
            var m = value / Math.Pow(1000, n);
            var unit = "";
 
            if (n < Units.Count)
            {
                unit = Units[n];
            }
            else
            {
                var unitInt = n - Units.Count;
                var secondUnit = unitInt % 26;
                var firstUnit = unitInt / 26;
                unit = Convert.ToChar(firstUnit + CharA) + Convert.ToChar(secondUnit + CharA).ToString();
            }
        
            return (Math.Floor(m * 100) / 100).ToString("0.##") + unit;
        }
    }
}
