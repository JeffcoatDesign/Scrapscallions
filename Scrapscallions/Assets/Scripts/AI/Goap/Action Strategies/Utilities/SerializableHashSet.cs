using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace Scraps.Utilities
{
    /*
     * Name: SerializableHashset
     * Source: azicMcAze https://github.com/azixMcAze/Unity-SerializableDictionary/blob/master/Assets/SerializableDictionary/SerializableHashSet.cs
     */
    public abstract class SerializableHashSetBase
    {
        public abstract class Storage { }

        protected class HashSet<TValue> : System.Collections.Generic.HashSet<TValue>
        {
            public HashSet() { }
            public HashSet(ISet<TValue> set) : base(set) { }
            public HashSet(SerializationInfo info, StreamingContext context) : base(info, context) { }
        }
    }

    [Serializable]
    public class SerializableHashSet<T> : SerializableHashSetBase, ISet<T>, ISerializationCallbackReceiver, IDeserializationCallback, ISerializable, IEnumerable where T : UnityEngine.Object
    {
        private HashSet<T> m_hashSet;
        [SerializeField] T[] m_keys;

        public IEnumerable<T> Values => m_hashSet;

        public SerializableHashSet()
        {
            m_hashSet = new HashSet<T>();
        }

        public SerializableHashSet(ISet<T> set)
        {
            m_hashSet = new HashSet<T>(set);
        }

        public void CopyFrom(ISet<T> set)
        {
            m_hashSet.Clear();
            foreach (var value in set)
            {
                m_hashSet.Add(value);
            }
        }

        internal void RemoveWhere(Predicate<T> match) => m_hashSet.RemoveWhere(match);

        public void OnAfterDeserialize()
        {
            if (m_keys != null)
            {
                m_hashSet.Clear();
                int n = m_keys.Length;
                for (int i = 0; i < n; ++i)
                {
                    m_hashSet.Add(m_keys[i]);
                }

                m_keys = null;
            }
        }

        public void OnBeforeSerialize()
        {
            int n = m_hashSet.Count;
            m_keys = new T[n];

            int i = 0;
            foreach (var value in m_hashSet)
            {
                m_keys[i] = value;
                ++i;
            }
        }

        #region ISet<TValue>

        public int Count => m_hashSet.Count;
        public bool IsReadOnly { get { return ((ISet<T>)m_hashSet).IsReadOnly; } }

        public bool Add(T item) => m_hashSet.Add(item);

        public void ExceptWith(IEnumerable<T> other)
        {
            ((ISet<T>)m_hashSet).ExceptWith(other);
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            ((ISet<T>)m_hashSet).IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return ((ISet<T>)m_hashSet).IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return ((ISet<T>)m_hashSet).IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return ((ISet<T>)m_hashSet).IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return ((ISet<T>)m_hashSet).IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return ((ISet<T>)m_hashSet).Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return ((ISet<T>)m_hashSet).SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            ((ISet<T>)m_hashSet).SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            ((ISet<T>)m_hashSet).UnionWith(other);
        }

        void ICollection<T>.Add(T item)
        {
            ((ISet<T>)m_hashSet).Add(item);
        }

        public void Clear() => m_hashSet.Clear();

        public bool Contains(T item) => m_hashSet.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((ISet<T>)m_hashSet).CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) => m_hashSet.Remove(item);

        public IEnumerator<T> GetEnumerator()
        {
            return ((ISet<T>)m_hashSet).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ISet<T>)m_hashSet).GetEnumerator();
        }

        #endregion

        #region IDeserializationCallback

        public void OnDeserialization(object sender)
        {
            ((IDeserializationCallback)m_hashSet).OnDeserialization(sender);
        }

        #endregion

        #region ISerializable

        protected SerializableHashSet(SerializationInfo info, StreamingContext context)
        {
            m_hashSet = new HashSet<T>(info, context);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)m_hashSet).GetObjectData(info, context);
        }
        #endregion
    }
}