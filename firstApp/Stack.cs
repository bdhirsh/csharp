using System;

namespace firstApp {
    class Stack<T> {

        Entry<T> top;
        int length;

        public void push(T val) {
            Entry<T> newTop = new Entry<T>(val, top);
            top = newTop;
            length++;
        }

        public T pop() {
            if (top == null) throw new InvalidOperationException();
            Entry<T> oldTop = top;
            top = top.next;
            length--;
            return oldTop.val;
        }
        
        public int size() {
            return length;
        }


        class Entry<T> {
            public Entry(T val, Entry<T> next) {
                this.val = val;
                this.next = next;
            }
            public T val;
            public Entry<T> next;
        }

    }
}