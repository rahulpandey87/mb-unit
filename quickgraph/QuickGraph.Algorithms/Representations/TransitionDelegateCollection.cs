using System;
using System.Collections;
namespace QuickGraph.Representations
{
    public class TransitionDelegateCollection : CollectionBase
    {
        public void Add(TransitionDelegate transition)
        {
            if (transition == null)
                throw new ArgumentNullException("transition");
            this.List.Add(transition);
        }
        public void Remove(TransitionDelegate transition)
        {
            if (transition == null)
                throw new ArgumentNullException("transition");
            this.List.Remove(transition);
        }
        public bool Contains(TransitionDelegate transition)
        {
            if (transition == null)
                throw new ArgumentNullException("transition");
            return this.List.Contains(transition);
        }
    }
}