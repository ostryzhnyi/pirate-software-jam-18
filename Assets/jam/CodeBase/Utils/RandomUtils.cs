using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectX.CodeBase.Utils
{
    public static class RandomUtils
    {
        public static float GetRandomRange(this Vector2 vector2)
        {
            return Random.Range(vector2.x, vector2.y);
        }
        
        public static Vector3 Get90AngleRandom()
        {
            var rand = Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    return new Vector3(0, 0, 0);
                case 1:
                    return new Vector3(0, 0, 90);
                case 2:
                    return new Vector3(0, 0, 180);
                case 3:
                    return new Vector3(0, 0, 270);
            }
            
            return Vector3.zero;
        }
        
        public static int GetRandomExcept(int from, int to, int except)
        {
            System.Random random = new System.Random();
            if (from >= to)
                throw new ArgumentException("from должно быть меньше to");
            
            if (except < from || except >= to)
                return random.Next(from, to);
        
            int range = to - from - 1;
            if (range <= 0)
                throw new ArgumentException("Недостаточно значений для выбора");
        
            int result = random.Next(from, to - 1);
        
            if (result >= except)
                result++;
            
            return result;
        }
        
        public static int GetRandom(int from, int to, int[] except)
        {
            System.Random random = new System.Random();
            
            if (from >= to)
                throw new ArgumentException("from должно быть меньше to");
            
            if (except == null || except.Length == 0)
                return random.Next(from, to);
        
            var validValues = new List<int>();
            for (int i = from; i < to; i++)
            {
                if (!Array.Exists(except, x => x == i))
                    validValues.Add(i);
            }
        
            if (validValues.Count == 0)
                throw new ArgumentException("Все значения исключены");
        
            return validValues[random.Next(validValues.Count)];
        }
        
        public static Vector3 Get90AngleRandom(Vector3 except)
        {
            var availableAngles = new List<Vector3>
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 90),
                new Vector3(0, 0, 180),
                new Vector3(0, 0, 270)
            };
   
            availableAngles.RemoveAll(angle => Mathf.Approximately(angle.z, except.z));
   
            if (availableAngles.Count == 0)
                return Vector3.zero;
   
            var rand = Random.Range(0, availableAngles.Count);
            return availableAngles[rand];
        }

        public static bool GetRandomBool()
        {
            return Random.Range(0, 2) == 0;
        }
    }
}