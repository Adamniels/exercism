public static class SaddlePoints
{
    public static IEnumerable<(int, int)> Calculate(int[,] matrix)
    {
        HashSet<(int, int)> result = new HashSet<(int, int)>();
        for (int rowIndex = 0; rowIndex < matrix.GetLength(0); rowIndex++)
        {
            int[] tallestTressIndexes = findTallestInRow(matrix, rowIndex);
            foreach (int columnIndex in tallestTressIndexes)
            {
                if (checkIfShortest(matrix, columnIndex, rowIndex))
                {
                    result.Add((rowIndex + 1, columnIndex + 1));
                }
            }
        }

        return result;
    }

    private static int[] findTallestInRow(int[,] matrix, int row)
    {
        int tallest = 0;
        List<int> indexTallest = new List<int>();

        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            int treeLength = matrix[row, i];

            if (treeLength > tallest)
            {
                tallest = treeLength;
                indexTallest.Clear();
                indexTallest.Add(i);
            }
            else if (treeLength == tallest)
            {
                indexTallest.Add(i);
            }
        }

        return indexTallest.ToArray();
    }

    private static bool checkIfShortest(int[,] matrix, int column, int row)
    {
        int treeToCheck = matrix[row, column];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            if (matrix[i, column] < treeToCheck)
            {
                return false;
            }
        }
        return true;
    }
}
