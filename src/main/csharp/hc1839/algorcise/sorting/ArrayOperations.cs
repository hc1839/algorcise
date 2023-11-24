using System;

namespace Hc1839
{
    namespace Algorcise
    {
        namespace Sorting
        {
            public class DictionaryEntryClass : IComparable
            {
                private readonly object objKey;
                private readonly object objValue;

                public DictionaryEntryClass(object Key, object Value)
                {
                    objKey = Key;
                    objValue = Value;
                }

                public object Key
                {
                    get
                    {
                        return this.objKey;
                    }
                }

                public object Value
                {
                    get
                    {
                        return this.objValue;
                    }
                }

                public int CompareTo(object obj)
                {
                    IComparable A = (IComparable) this.objValue;    // Value property of this object.
                    object B;    // Argument for CompareTo method of object A.

                    if (obj is DictionaryEntryClass)
                    {
                        B = ((DictionaryEntryClass) obj).Value;
                    }
                    else
                    {
                        B = obj;
                    }

                    return A.CompareTo(B);
                }
            }

            public class ArrayOperations
            {
                private static void InsertSort(IComparable[] SourceArray, int LoLim, int HiLim)
                {
                    IComparable[] A = SourceArray;
                    int L = LoLim;    // Left limit.
                    int R = HiLim;    // Right limit.
                    int LP;    // Left "pointer".
                    int RP;    // Right "pointer".
                    IComparable LT; IComparable RT;    // Temporary variables for comparing elements.

                    for (RP = L + 1; RP <= R; RP++)
                    {
                        RT = A[RP];

                        LP = RP - 1;
                        while (true)
                        {
                            LT = A[LP];
                            if (RT.CompareTo(LT) < 0)
                            {
                                A[LP + 1] = A[LP];
                                if (LP > L) LP--; else break;
                            }
                            else
                            {
                                LP++;
                                break;
                            }
                        }

                        A[LP] = RT;
                    }
                }

                public static void MergeSort(Array SourceArray, int LoLim, int HiLim)
                {
                    const int N = 16;    // Arbitrary maximum number of elements for InsertSort.
                    int LP;    // Left "pointer".
                    int RP;    // Right "pointer".
                    int OP;    // Output "pointer".
                    int Length = HiLim - LoLim + 1;    // Number of elements to sort.
                    IComparable LT; IComparable RT;    // Temporary variables for comparing elements.
                    IComparable[] A;    // Source array.
                    IComparable[] B;    // Buffer array.
                    IComparable[] C;    // Temporary variable for switching source and buffer arrays.

                    // Cast SourceArray elements into array A.
                    A = (IComparable[]) Array.CreateInstance(typeof(IComparable), Length);
                    for (int I = 0; I <= Length - 1; I++)
                    {
                        A[I] = (IComparable) SourceArray.GetValue(I + LoLim);
                    }

                    // InsertSort sub-arrays of N elements.
                    for (int I = 0; I <= Length - 1; I += N)
                    {
                        ArrayOperations.InsertSort(A, I, Math.Min(I + N - 1, Length - 1));
                    }

                    // Stagger the sub-arrays. This allows the merging to be shuttled.
                    B = (IComparable[]) Array.CreateInstance(typeof(IComparable), Length);
                    for (int I = 2*N; I <= Length - 1; I += 2*N)
                    {
                        for (int J = I; J <= Math.Min(I + N - 1, Length - 1); J++)
                        {
                            B[J] = A[J];
                        }
                    }

                    // Merge the sub-arrays.
                    for (int I = N; I <= Length - 1; I += N)
                    {
                        LP = 0; RP = I; OP = 0;
                        while (LP < I & RP < Math.Min(I + N, Length))
                        {
                            LT = A[LP]; RT = A[RP];
                            if (RT.CompareTo(LT) < 0)
                            {
                                B[OP] = RT;
                                RP++;
                            }
                            else
                            {
                                B[OP] = LT;
                                LP++;
                            }
                            OP++;
                        }

                        // Either the left or right pointer has ran out its sub-array.
                        // Transfer the rest of the remaining sub-array.
                        {
                            int J = (LP >= I ? RP : LP);
                            int K = (LP >= I ? Math.Min(I + N - 1, Length - 1) : I - 1);
                            for (; J <= K; J++)
                            {
                                B[OP] = A[J];
                                OP++;
                            }
                        }

                        // Switch source and buffer arrays.
                        C = A;
                        A = B;
                        B = C;
                    }

                    // Put elements of array A back into SourceArray.
                    for (int I = 0; I <= Length - 1; I++)
                    {
                        SourceArray.SetValue(A[I], I + LoLim);
                    }
                }

                public static void MatrixMergeSort(Array Matrix, int LoRowIndex, int HiRowIndex)
                {
                    object[] A;    // One-dimensional representation of Matrix.
                    DictionaryEntryClass[] B;    // Buffer array for MergeSort.
                    int[] C;    // Array of indices.
                    int nRows = HiRowIndex - LoRowIndex + 1;    // Number of rows to sort.
                    int OP;    // Output "pointer" for array A.

                    // Create a one-dimensional representation of Matrix.
                    A = new object[(Matrix.GetUpperBound(1) - Matrix.GetLowerBound(1) + 1) * nRows];
                    OP = 0;
                    for (int J = Matrix.GetLowerBound(1); J <= Matrix.GetUpperBound(1); J++)
                    {
                        for (int I = LoRowIndex; I <= HiRowIndex; I++)
                        {
                            A[OP] = Matrix.GetValue(I, J);
                            OP++;
                        }
                    }

                    // Prepare the buffer array.
                    B = new DictionaryEntryClass[nRows];
                    for (int I = 0; I <= nRows - 1; I++)
                    {
                        B[I] = new DictionaryEntryClass((int) (I + A.GetUpperBound(0) + 1), null);
                    }

                    // MergeSort individual columns from right to left.
                    while (((int) (B[0].Key)) >= nRows)
                    {
                        int TMP;
                        for (int I = 0; I <= nRows - 1; I++)
                        {
                            TMP = (int) B[I].Key; TMP -= nRows;
                            B[I] = new DictionaryEntryClass(TMP, A[TMP]);
                        }

                        ArrayOperations.MergeSort(B, 0, nRows - 1);
                    }

                    // Create array C from array B.
                    C = new int[nRows];
                    for (int I = 0; I <= nRows - 1; I++)
                    {
                        C[I] = (int) B[I].Key;
                    }
                    B = null;

                    // Put elements of array A back into Matrix via array C.
                    {
                        int TMP = 0;    // Index shifting.
                        for (int J = Matrix.GetLowerBound(1); J <= Matrix.GetUpperBound(1); J++)
                        {
                            for (int I = LoRowIndex; I <= HiRowIndex; I++)
                            {
                                Matrix.SetValue(A[C[I - LoRowIndex] + TMP], I, J);
                            }

                            TMP += nRows;
                        }
                    }
                }
            }
        }
    }
}
