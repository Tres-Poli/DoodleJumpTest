using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class ListQueue<T> : List<T>
{
    public void Enqueue(T element)
    {
        Add(element);
    }

    public T Dequeue()
    {
        var result = base[0];
        RemoveAt(0);
        return result;
    }

    public T Peek()
    {
        return base[0];
    }
}
