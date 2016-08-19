using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyStyleApp.Models
{
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }
        public string ShortKey { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            ShortKey = "?";
            if (Key != null && !string.IsNullOrEmpty(Key.ToString()))
            {
                ShortKey = key.ToString().Substring(0, 1);
            }

            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
