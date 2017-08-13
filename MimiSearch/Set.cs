using System;
using System.Collections;
using System.Collections.Generic;

namespace MimiSearch
{
    internal class Set
    {
        private Stack<string> _s;

        public Set()
        {
            _s = new Stack<string>();
        }


        public void Add(string str)
        {
            if (!(_s.Contains(str)))
            {
                _s.Push(str);
            }
        }

        public string Pop()
        {
            return _s.Pop();
        }

        internal bool Contains(string str)
        {
            return _s.Contains(str);
        }

        internal string[] ToArray()
        {
            return _s.ToArray();
        }
    }
}