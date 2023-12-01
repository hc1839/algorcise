namespace Hc1839.Algorcise.Sorting;

public class ArrayOperations
{
    public static void InsertSort<T>(
        T[] sourceArray,
        int loLim,
        int hiLim,
        Comparison<T> comparison
    )
    {
        T[] a = sourceArray;

        for (int rp = loLim + 1; rp <= hiLim; rp++)
        {
            // Temporary variable for comparing elements.
            T rt = a[rp];
            // Left "pointer".
            int lp = rp - 1;

            while (true)
            {
                // Temporary variable for comparing elements.
                T lt = a[lp];

                if (comparison(rt, lt) < 0)
                {
                    a[lp + 1] = a[lp];
                    if (lp > loLim) {
                        lp--;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    lp++;
                    break;
                }
            }

            a[lp] = rt;
        }
    }

    public static void MergeSort<T>(
        T[] sourceArray,
        int loLim,
        int hiLim,
        Comparison<T> comparison,
        int insertSortSize = 16
    )
    {
        if (insertSortSize < 2)
        {
            throw new ArgumentException(
                "Size of sub-array for insert sort is not at least 2."
            );
        }

        // Number of elements to sort.
        int length = hiLim - loLim + 1;

        // Cast sourceArray elements into array a.
        var a = new T[length];

        for (int i = 0; i <= length - 1; i++)
        {
            a[i] = sourceArray[i + loLim];
        }

        // InsertSort sub-arrays of insertSortSize elements.
        for (int i = 0; i <= length - 1; i += insertSortSize)
        {
            InsertSort(
                a,
                i,
                Math.Min(i + insertSortSize - 1, length - 1),
                comparison
            );
        }

        // Stagger the sub-arrays. This allows the merging to be shuttled.
        var b = new T[length];

        for (
            int i = 2 * insertSortSize;
            i <= length - 1;
            i += 2 * insertSortSize
        )
        {
            for (
                int j = i;
                j <= Math.Min(i + insertSortSize - 1, length - 1);
                j++
            )
            {
                b[j] = a[j];
            }
        }

        // Merge the sub-arrays.
        for (int i = insertSortSize; i <= length - 1; i += insertSortSize)
        {
            // Left "pointer".
            int lp = 0;
            // Right "pointer".
            int rp = i;
            // Output "pointer".
            int op = 0;

            while (lp < i & rp < Math.Min(i + insertSortSize, length))
            {
                // Temporary variables for comparing elements.
                T lt = a[lp];
                T rt = a[rp];

                if (comparison(rt, lt) < 0)
                {
                    b[op] = rt;
                    rp++;
                }
                else
                {
                    b[op] = lt;
                    lp++;
                }

                op++;
            }

            // Either the left or right pointer has ran out its sub-array.
            // Transfer the rest of the remaining sub-array.
            {
                int j = (lp >= i ? rp : lp);
                int k;

                if (lp >= i)
                {
                    k = Math.Min(i + insertSortSize - 1, length - 1);
                }
                else
                {
                    k = i - 1;
                }

                for (; j <= k; j++)
                {
                    b[op] = a[j];
                    op++;
                }
            }

            // Switch source and buffer arrays.
            T[] c = a;
            a = b;
            b = c;
        }

        // Put elements of array a back into sourceArray.
        for (int i = 0; i <= length - 1; i++)
        {
            sourceArray[i + loLim] = a[i];
        }
    }

    public static void MatrixMergeSort<T>(
        T[,] matrix,
        int loRowIndex,
        int hiRowIndex,
        Comparison<T> comparison
    )
    {
        // Number of rows to sort.
        int nRows = hiRowIndex - loRowIndex + 1;

        // Create a one-dimensional representation of matrix.
        var a = new T[
            (matrix.GetUpperBound(1) - matrix.GetLowerBound(1) + 1) * nRows
        ];
        // Output "pointer" for array a.
        int op = 0;

        for (
            int j = matrix.GetLowerBound(1);
            j <= matrix.GetUpperBound(1);
            j++
        )
        {
            for (int i = loRowIndex; i <= hiRowIndex; i++)
            {
                a[op] = matrix[i, j];
                op++;
            }
        }

        // Prepare the buffer array.
        var b = new KeyValuePair<int, T>[nRows];

        // MergeSort individual columns from right to left.
        while (a.GetUpperBound(0) + 1 >= nRows)
        {
            for (int i = 0; i <= nRows - 1; i++)
            {
                int tmp = i + a.GetUpperBound(0) + 1 - nRows;
                b[i] = new KeyValuePair<int, T>(tmp, a[tmp]);
            }

            MergeSort(
                b,
                0,
                nRows - 1,
                (x, y) => { return comparison(x.Value, y.Value); }
            );
        }

        // Create array c from array b.
        var c = new int[nRows];

        for (int i = 0; i <= nRows - 1; i++)
        {
            c[i] = b[i].Key;
        }

        b = null;

        // Put elements of array a back into matrix via array c.
        {
            for (
                int j = matrix.GetLowerBound(1);
                j <= matrix.GetUpperBound(1);
                j++
            )
            {
                // Index shifting.
                int tmp = 0;

                for (int i = loRowIndex; i <= hiRowIndex; i++)
                {
                    matrix[i, j] = a[c[i - loRowIndex] + tmp];
                }

                tmp += nRows;
            }
        }
    }
}
