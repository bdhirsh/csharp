using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace practice {

    class SortNegative : Comparer<int> {
        public override int Compare(int x, int y) {
            return x < y ? -1 : x > y ? 1 : 0;
        }
    }

    static class Extension {
        // handy general purpose dictionary toString method
        public static string ToDebugString<TKey, TValue> (this IDictionary<TKey, TValue> dictionary) {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }

        // dictionary toString method, specific to when value is a list
        public static string ToDebugString<TKey, TValue> (this IDictionary<TKey, List<TValue>> dictionary) {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value.ToDebugString()).ToArray()) + "}";
        }

        // list toString method
        public static string ToDebugString<T> (this IList<T> list) {
            return "{" + string.Join(", ", list.ToArray()) + "}";
        }
    }

    class StringStore {

        private string[] arr;

        public StringStore(int n) {
            if (n < 0) throw new IndexOutOfRangeException("StringStore must have a positive length");
            arr = new string[n];
        }

        // inefficiently resizes the "store" array
        // used as an example of get/set properties
        public int size {
            get { return arr.Length; }
            set {
                if (value < 0) throw new IndexOutOfRangeException("StringStore must have a positive length");
                string[] tmp = new string[value];
                for (int i = 0; i < Math.Min(arr.Length, value); i++) {
                    tmp[i] = arr[i];
                }
                arr = tmp;
            }
        }

        public string this[int index] {

            get {
                if (index < 0 || index >= arr.Length) {
                    throw new IndexOutOfRangeException("cannot store more than {arr.Length} objects");
                }
                return arr[index];
            }
            
            set {
                if (index < 0 || index >= arr.Length) {
                    throw new IndexOutOfRangeException("cannot store more than {arr.Length} objects");
                }
                arr[index] = value;
            }
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder("{");
            for (int i = 0; i < arr.Length; i++) {
                if (i == arr.Length - 1) {
                    builder.Append(arr[i]);
                } else {
                    builder.Append(arr[i] + ", ");
                }
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}