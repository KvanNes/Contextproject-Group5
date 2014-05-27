using System;
using System.Collections.Generic;

namespace AssemblyCSharp {

    public class CompositeKeyedDictionary<S, T, U> {

        private Dictionary<S, Dictionary<T, U>> dictionary;

        public CompositeKeyedDictionary() {
            dictionary = new Dictionary<S, Dictionary<T, U>>();
        }

        public U Get(S s, T t) {
            Dictionary<T, U> subDictionary = Utils.getDictionaryValue(dictionary, s);
            if (subDictionary == null) {
                return default(U);
            }

            return Utils.getDictionaryValue(subDictionary, t);
        }

        public void Set(S s, T t, U u) {
            Dictionary<T, U> subDictionary = Utils.getDictionaryValue(dictionary, s);
            if (subDictionary == null) {
                subDictionary = new Dictionary<T, U>();
                dictionary.Add(s, subDictionary);
            }
            if(subDictionary.ContainsKey(t)) {
                subDictionary.Remove(t);
            }
            subDictionary.Add(t, u);
        }

        public void Remove(S s) {
            dictionary.Remove(s);
        }
        
        public void Remove(T t) {
            foreach (KeyValuePair<S, Dictionary<T, U>> pair in dictionary) {
                if (pair.Value.ContainsKey(t)) {
                    pair.Value.Remove(t);
                }
            }
        }
        
        public void Remove(U u) {
            foreach (KeyValuePair<S, Dictionary<T, U>> pair in dictionary) {
                foreach(KeyValuePair<T, U> subPair in pair.Value) {
                    if (subPair.Value.Equals(u)) {
                        pair.Value.Remove(subPair.Key);
                    }
                }
            }
        }
    }
}
