using System.Collections;

public class BinarySearchTree : IEnumerable<int>
{
    private int _value;
    private BinarySearchTree? _left;
    private BinarySearchTree? _right;

    public BinarySearchTree(int value)
    {
        this._value = value;

    }

    public BinarySearchTree(IEnumerable<int> values)
    {
        using var enumerator = values.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            throw new Exception("wrong in enumerator");
        }

        this._value = enumerator.Current;

        while (enumerator.MoveNext())
        {
            this.Add(enumerator.Current);

        }
    }

    public int Value => _value;
    public BinarySearchTree? Left => _left;
    public BinarySearchTree? Right => _right;

    public BinarySearchTree Add(int value)
    {
        if (value <= this._value)
        {
            if (_left == null)
            {
                _left = new BinarySearchTree(value);
            }
            else
            {
                _left.Add(value);
            }
        }
        else if (value > this._value)
        {
            if (_right == null)
            {
                _right = new BinarySearchTree(value);
            }
            else
            {
                _right.Add(value);
            }
        }
        return this;
    }

    public IEnumerator<int> GetEnumerator()
    {
        if (Left != null)
            foreach (var leftValue in Left)
                yield return leftValue;

        yield return Value;

        if (Right != null)
            foreach (var rightValue in Right)
                yield return rightValue;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
