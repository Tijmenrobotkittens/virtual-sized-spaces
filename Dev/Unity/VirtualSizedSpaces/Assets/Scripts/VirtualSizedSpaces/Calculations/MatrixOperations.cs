using UnityEngine;

public static class MatrixOperations
{
    public static float[,] multiply(this float[,] a, float[,] b)
    {
        if (a.GetLength(1) != b.GetLength(0))
            throw new System.Exception("Ehhh, fout in de matrix");

        int M = a.GetLength(0);
        int L = a.GetLength(1);
        int N = b.GetLength(1);

        var res = new float[M, N];
        for (int m = 0; m < M; ++m)
        {
            for (int n = 0; n < N; ++n)
            {
                float s = 0;
                for (int l = 0; l < L; ++l)
                {
                    s += a[m, l] * b[l, n];
                }

                res[m, n] = s;
            }
        }
        return res;
    }

    public static float[] multiply(this float[,] a, float[] b)
    {
        if (a.GetLength(1) != b.Length)
            throw new System.Exception("Ehhh, fout in de matrix");

        int M = a.GetLength(0);
        int L = b.Length;

        var res = new float[M];
        for (int m = 0; m < M; ++m)
        {
            float s = 0;
            for (int l = 0; l < L; ++l)
            {
                s += a[m, l] * b[l];
            }
            res[m] = s;
        }
        return res;
    }
}
