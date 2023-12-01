namespace Hc1839.Algorcise.Sorting;

public class DictionaryEntryClass : IComparable
{
    private readonly object _objKey;
    private readonly object _objValue;

    public DictionaryEntryClass(object key, object value)
    {
        _objKey = key;
        _objValue = value;
    }

    public object Key
    {
        get { return this._objKey; }
    }

    public object Value
    {
        get { return this._objValue; }
    }

    public int CompareTo(object obj)
    {
        // Value property of this object.
        IComparable a = (IComparable) this._objValue;

        if (obj is DictionaryEntryClass)
        {
            return a.CompareTo(((DictionaryEntryClass) obj).Value);
        }
        else
        {
            return a.CompareTo(obj);
        }
    }
}
