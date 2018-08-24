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


        class Entry<S> {
            public Entry(S val, Entry<S> next) {
                this.val = val;
                this.next = next;
            }
            public S val;
            public Entry<S> next;
        }

    }
}