using System;
using System.Collections.Generic;

public class ItemSelector<T> where T : notnull
{
    private readonly Random random;
    private readonly Dictionary<T, int> entries;
    private int totalWeight;

    public int TotalWeight { get { return totalWeight; } }

    public int Size { get { return entries.Count; } }

    public bool Empty { get { return Size == 0; } }

    public ItemSelector()
    {
        entries = new Dictionary<T, int>();
        totalWeight = 0;
        random = new Random();
    }
    
    public ItemSelector(int seed)
    {
        entries = new Dictionary<T, int>();
        totalWeight = 0;
        random = new Random(seed);
    }

    public void AddItem(T item, int weight)
    {
        if (entries.ContainsKey(item))
        {
            RemoveItem(item);
        }

        if (weight <= 0)
        {
            throw new ArgumentException("Weight must be greater than 0");
        }

        entries[item] = weight;
        totalWeight += weight;
    }

    public void RemoveItem(T item)
    {
        if (!entries.TryGetValue(item, out _))
            return;

        totalWeight -= entries[item];
        entries.Remove(item);
    }

    public long Weight(T item)
    {
        if (!Contains(item))
            return -1;

        return entries[item];
    }

    public void SetWeight(T item, int weight)
    {
        if (!Contains(item))
            return;
        
        RemoveItem(item);
        AddItem(item, weight);
    }
    
    public bool Contains(T item)
    {
        return entries.ContainsKey(item);
    }

    public T Roll()
    {
        long value = random.Next(totalWeight);

        foreach (var item in entries.Keys)
        {
            long weight = entries[item];
            value -= weight;

            if (value < 0)
            {
                return item;
            }
        }

        throw new Exception("Impossible state");
    }

    public float Chance(T item)
    {
        if (!Contains(item))
            return 0;

        return (float)entries[item] / totalWeight;
    }        
}