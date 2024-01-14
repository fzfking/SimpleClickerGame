using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface IRepository<T> where T: IEquatable<T>
    {
        IReadOnlyCollection<T> Items { get; }
        void Add(T item);
        bool Remove(T item);
        bool Has(T item);
    }
    
    public class Repository<T>: IRepository<T> where T: IEquatable<T>
    {
        private readonly List<T> _items = new();

        public IReadOnlyCollection<T> Items => _items;

        public void Add(T item)
        {
            _items.Add(item);
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        public bool Has(T item)
        {
            return _items.Contains(item);
        }
    }
}