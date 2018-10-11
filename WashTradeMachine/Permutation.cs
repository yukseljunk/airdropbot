using System.Collections.Generic;

namespace WashTradeMachine
{
    public class Permutation
    {

        private static void Swap(ref int a, ref int b)
        {
            if (a == b) return;
            int c;
            c = a;
            a = b;
            b = c;
        }

        public static void GetPer(int[] list, List<List<int>> permutations)
        {
            int x = list.Length - 1;
            GetPer(list, 0, x, permutations);

        }

        private static void GetPer(int[] list, int k, int m, List<List<int>> permutations)
        {
            if (k == m)
            {
                permutations.Add(new List<int>(list));
            }
            else
                for (int i = k; i <= m; i++)
                {
                    Swap(ref list[k], ref list[i]);
                    GetPer(list, k + 1, m, permutations);
                    Swap(ref list[k], ref list[i]);
                }
        }

    }
}