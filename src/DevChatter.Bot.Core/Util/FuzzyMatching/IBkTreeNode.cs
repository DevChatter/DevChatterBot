
using System;

namespace DevChatter.Bot.Core.Util.FuzzyMatching
{
    /**
     * A node in a {@link BkTree}.
     *
     * @param <E> type of elements in the tree to which this node belongs
     */
    public interface IBkTreeNode<out TKey, out TValue>
    {
        /** Returns the element in this node. */
        TKey Key { get; }
        
        TValue Value { get; }

        /** Returns the child node at the given distance, if any. */
        IBkTreeNode<TKey, TValue> GetChildNode(Int32 distance);
    }
}