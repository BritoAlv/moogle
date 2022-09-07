public static class sort
{
        // i will do quicksort in the double values.
    static void swap(Tuple<int, double>[] A, int i, int j)
    {
            Tuple<int, double> temp = A[i];
            A[i] = A[j];
            A[j] = temp;
    }

    static int partition(Tuple<int, double>[] A, int low, int high)
    {
  
        // pivot
        Tuple<int, double> pivot = A[high];
  
        // Index of smaller element and
        // indicates the right position
        // of pivot found so far
        int i = (low - 1);
  
        for (int j = low; j <= high - 1; j++)
        {
        // If current element is smaller 
        // than the pivot
            if (A[j].Item2 < pivot.Item2)
            {
                // Increment index of 
                // smaller element
                i++;
                swap(A, i, j);
            }     
        }
        swap(A, i + 1, high);
        return (i + 1);
        
    }

    public static void quickSort(Tuple<int, double>[] A, int low, int high)
    {
        if (low < high)
        {
  
            // pi is partitioning index, arr[p]
            // is now at right place 
            int pi = partition(A, low, high);
  
            // Separately sort elements before
            // partition and after partition
            quickSort(A, low, pi - 1);
            quickSort(A, pi + 1, high);
        }
    }
}