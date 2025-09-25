public class Matrix
{
    private List<List<int>> _matrix;

    public Matrix(string input)
    {
        // exempel sträng - "1 2\n3 4"
        _matrix = new List<List<int>>();
        string[] rowString = input.Split('\n');
        foreach (string row in rowString)
        {
            string[] stringArray = row.Split(' ');
            List<int> intList = stringArray.Select(int.Parse).ToList();
            _matrix.Add(intList);
        }

    }

    public int[] Row(int row)
    {
        return _matrix[row - 1].ToArray();
    }

    public int[] Column(int col)
    {
        return _matrix.Select(row => row[col - 1]).ToArray();
    }
}
