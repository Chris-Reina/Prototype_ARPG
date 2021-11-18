using System;
using System.Collections;
using System.Collections.Generic;

public class Matrix<T> : IEnumerable<T>
{
	public int Width { get; }
	public int Height { get; }
	public int Capacity { get; }

	public bool IsEmpty => _internal.Length == 0;

	private T[] _internal;

    public Matrix(int width, int height)
    {
	    if (width < 1 || height < 1) throw new IndexOutOfRangeException();
	    
	    _internal = new T[width * height];
        Width = width;
        Height = height;
        Capacity = width * height;
    }

	public Matrix(T[,] copyFrom)
	{
		Width = copyFrom.GetLength(1);
		Height = copyFrom.GetLength(0);
		Capacity = Width * Height;
		
		_internal = new T[copyFrom.GetLength(0) * copyFrom.GetLength(1)];

		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				_internal[j + i * Width] = copyFrom[j, i];
			}
		}
	}

	public Matrix(Matrix<T> copyFrom)
	{
		Width = copyFrom.Width;
		Height = copyFrom.Height;
		Capacity = Width * Height;

		_internal = new List<T>(copyFrom._internal).ToArray();
	}

	public Matrix<T> Clone() 
	{
        var aux = new Matrix<T>(Width, Height);
        
        for (int i = 0; i < _internal.Length; i++)
        {
	        aux._internal[i] = _internal[i];
        }
        
        return aux;
    }

	public T this[int x, int y]
    {
	    get
	    {
		    if (x < 0 || x > Width || y < 0 || y > Height)
		    {
			    throw new IndexOutOfRangeException();
		    }

		    return _internal[x + y * Width];
	    }
	    set
	    {

		    if (x < 0 || x > Width || y < 0 || y > Height)
		    {
			    throw new IndexOutOfRangeException();
		    }
		    
		    _internal[x + y * Width] = value;
	    }
    }

    public IEnumerator<T> GetEnumerator()
    {
	    for (int i = 0; i < Capacity; i++)
	    {
		    yield return _internal[i];
	    }
    }

    IEnumerator IEnumerable.GetEnumerator() 
    {
	    return GetEnumerator();
    }

    public override string ToString()
    {
	    var ser = "";
	    
	    for (var i = 0; i < Height; i++)
	    {
		    var auxSer = "";
		    for (var j = 0; j < Width; j++)
		    {
			    auxSer += $"{_internal[j + i * Width]},";
		    }

		    ser += auxSer + "\n";
	    }

	    return ser;
    }
}
