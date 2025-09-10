public class CircularBuffer<T>
{

    private T[] _buffer;
    private int _nextIndex = 0;
    private int _oldestIndex = 0;
    private int _count = 0;

    public int Capacity { get; }
    public int Count => _count;
    public bool IsEmpty => _count == 0;
    public bool IsFull => _count == Capacity;

    public CircularBuffer(int capacity)
    {
        Capacity = capacity;
        _buffer = new T[capacity];
    }


    // Remove and returns the first one 
    public T Read()
    {
        if (_count == 0)
        {
            throw new InvalidOperationException("Can't read a empty buffer");
        }

        T element = _buffer[_oldestIndex % Capacity];
        _oldestIndex = (_oldestIndex + 1) % Capacity;

        _count--;

        return element;
    }

    public void Write(T value)
    {

        if (_count == Capacity)
        {
            throw new InvalidOperationException("Can't write to a full buffer, use Overwrite if intentional");
        }

        _buffer[_nextIndex] = value;

        _nextIndex = (_nextIndex + 1) % Capacity;

        _count++;

    }

    public void Overwrite(T value)
    {
        if (_count != Capacity)
        {
            Write(value);
        }
        else
        {
            _buffer[_nextIndex] = value;

            _nextIndex = (_nextIndex + 1) % Capacity;
            _oldestIndex = (_oldestIndex + 1) % Capacity;
        }
    }

    public void Clear()
    {
        _nextIndex = 0;
        _oldestIndex = 0;
        _count = 0;

    }
}
