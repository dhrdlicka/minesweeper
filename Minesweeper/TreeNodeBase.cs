using System.Collections.ObjectModel;

namespace Minesweeper;

public interface ITreeNode<T> where T : class, ITreeNode<T>
{
    T? Parent { get; set; }
    T? Root { get; }
    ICollection<T> Children { get; }
}

public abstract class TreeNodeBase<T> : ITreeNode<T> where T : TreeNodeBase<T>
{
    public event EventHandler? ParentChanged;
    protected void OnParentChanged(EventArgs e) => ParentChanged?.Invoke(this, e);

    T? _parent;
    public T? Parent
    {
        get => _parent;
        set
        {
            if (value == Parent)
            {
                return;
            }

            if (Parent is not null)
            {
                var oldParent = Parent;
                _parent = null;

                if (oldParent.Children.Contains(this))
                {
                    oldParent.Children.Remove((T)this);
                }

                if (value is null)
                {
                    OnParentChanged(new());
                }
            }

            if (value is not null)
            {
                if (!value.Children.Contains(this))
                {
                    value.Children.Add((T)this);
                }

                _parent = value;
                OnParentChanged(new());
            }
        }
    }

    public T? Root
    {
        get
        {
            T? root = Parent;
            while (root is not null and { Parent: not null })
            {
                root = root.Parent;
            }
            return root;
        }
    }

    public ICollection<T> Children { get; }

    protected TreeNodeBase()
    {
        Children = new TreeNodeBase<T>.ChildrenCollection((T)this);
    }

    private class ChildrenCollection : Collection<T>
    {
        public T Parent { get; }

        public ChildrenCollection(T control)
        {
            Parent = control;
        }

        protected override void ClearItems()
        {
            while(this.Count > 0)
            {
                RemoveAt(0);
            }
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            item.Parent = Parent;
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);
            item.Parent = null;
        }

        protected override void SetItem(int index, T item)
        {
            var oldItem = this[index];
            base.SetItem(index, item);
            oldItem.Parent = null;
            item.Parent = Parent;
        }
    }
}
