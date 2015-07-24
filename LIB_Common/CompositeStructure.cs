using System.Collections.Generic;

namespace LIB_Common
{
    public interface IGetOneLeafChild<T>
    {
        string GetOneLeafChild(string fullname);
    }
    /// <summary>
    /// Enum for Composite Type
    /// </summary>
    public enum ECompositeType
    {
        root=10,
        branch=20,
        leaf=30
    }
    /// <summary>
    /// Interface of Composite 
    /// </summary>
    /// <typeparam name="T">Class of the node</typeparam>
    public interface IComposite<T>
    {
        T Node { get; set; }
        IComposite<T> Parent { get; set; }

        string Name { get; set; }

        string FullName { get; set; }

        string Separator { get; set; }

        int Level { get; set; }

        ECompositeType Compositetype { get; set; }

        bool Add(IComposite<T> composite);
        bool AddChildByFullName(IComposite<T> composite);
        bool Remove(IComposite<T> composite);

        IDictionary<string, IComposite<T>> GetAllChild();

        IComposite<T> GetChild(string fullname,bool isRecursive);

        IComposite<T> GetFristChild(string Name, bool isRecursive);

        IComposite<T> GetParent();

        string GetFullName();

        string FunName_GetOneLeafChild { get; set; }

        string GetOneLeafChild(Dictionary<string,IGetOneLeafChild<T>> Dict_Fun);
    }

    /// <summary>
    /// Basic implementation of CompositeStructure
    /// </summary>
    /// <typeparam name="T">Class of the node</typeparam>
    public class CompositeStructure<T> : IComposite<T>
    {
        private IComposite<T> _parent;
        private string _name;
        private string _fullName;
        private string _separator;
        private int _level;
        private string _funName_GetOneLeafChild;


        private ECompositeType _compositetype;
        private Dictionary<string, IComposite<T>> _childDict;
        private T _node;

        #region IComposite<T> Members
        public T Node
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
            }
        }


        public IComposite<T> Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string FullName
        {
            get
            {
                return _fullName;
            }
            set
            {
                _fullName = value;
            }
        }

        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        public ECompositeType Compositetype
        {
            get
            {
                return _compositetype;
            }
            set
            {
                _compositetype = value;
            }
        }

        public string Separator
        {
            get
            {
                return _separator;
            }
            set
            {
                _separator = value;
            }
        }

        public Dictionary<string, IComposite<T>> ChildDict
        {
            get
            {
                return _childDict;
            }
            set
            {
                _childDict = value;
            }
        }

        public string FunName_GetOneLeafChild
        {
            get { return _funName_GetOneLeafChild; }
            set { _funName_GetOneLeafChild = value; }
        }
        
        #endregion

        public CompositeStructure(T node,string name, int level, ECompositeType compositetype)
        {
            _fullName = null;
            _node = node;
            _name = name;
            _level = level;
            _compositetype = compositetype;
            _separator = ".";
            if (Compositetype == ECompositeType.root)
            {
                _parent = null;
                _fullName = _name;
            }
            _childDict = new Dictionary<string, IComposite<T>>();
        }

        public bool Add(IComposite<T> composite)
        {
            if (composite == null)
            {
                return false ;
            }
            else
            {
                if (Compositetype == ECompositeType.leaf)
                {
                    return false;
                }
                if (composite.Compositetype == ECompositeType.root)
                {
                    return false;
                }

                if ((composite.Level != (Level + 1)) || (composite.Level == 0))
                {
                    return false;
                }
                if (composite.FullName != null)
                {
                    return false;
                }

                var newFullName = FullName + Separator + composite.Name;
                if (!ChildDict.ContainsKey(newFullName))
                {
                    composite.Parent = this;
                    composite.FullName = newFullName;
                    composite.Level = Level + 1;
                    ChildDict.Add(composite.FullName, composite);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool AddChildByFullName(IComposite<T> composite)
        {
            var result = false;
            if (composite!=null&&composite.FullName != null && composite.FullName != "")
            {
                if (composite.FullName != null && composite.FullName != "")
                {
                    var index = composite.FullName.LastIndexOf(Separator);

                    IComposite<T> pComposite;
                    if (index >= 0)
                    {
                        var pCompositeFullName = composite.FullName.Substring(0, index);
                        pComposite = GetChild(pCompositeFullName, true);
                    }
                    else
                    {
                        pComposite = this;
                    }

                    if (pComposite != null)
                    {
                        composite.FullName = "";
                        pComposite.Add(composite);
                    }
                    result = true;
                }
                else
                {
                    Add(composite);
                    result = true;
                }
            }

            return result;
        }

        public bool Remove(IComposite<T> composite)
        {
            if (ChildDict!=null&&ChildDict.Count>0)
            {
                if (composite!=null&&composite.FullName != null)
                    return ChildDict.Remove(composite.FullName);
            }
            return false;
        }

        public IComposite<T> GetChild(string fullname, bool isRecursive)
        {
            IComposite<T> composite;

            if (ChildDict.Count <= 0)
                return null;
            var pIndex = fullname.IndexOf(FullName);
            if (pIndex < 0)
                return null;
            var nextIndex = fullname.IndexOf(Separator, FullName.Length + pIndex +1);
            if (nextIndex < 0)
                nextIndex = fullname.Length;
            var pchildFullName = fullname.Substring(0, nextIndex);
            var result = ChildDict.TryGetValue(pchildFullName, out composite);
            if (result)
            {
                if (pchildFullName == fullname)
                {
                    return composite;
                }
                else
                {
                    if (isRecursive)
                        return composite.GetChild(fullname, isRecursive);
                    else
                        return null;
                }
            }
            return composite;

            //composite = com.GetChild(fullname, true);

            //foreach (IComposite<T> com in ChildDict.Values)
            //{
            //    composite = com.GetChild(fullname, true);
            //}

            //if (ChildDict.TryGetValue(fullname, out composite))
            //    return composite;

            //if (ChildDict == null)
            //    return null;

            //if (isRecursive)
            //{
            //    foreach (IComposite<T> com in ChildDict.Values)
            //    {
            //        composite = com.GetChild(fullname, true);
            //        if (composite != null)
            //            return composite;
            //    }
            //}
            //else
            //{
            //    composite = null;
            //}
            //return composite;
        }

        public IComposite<T> GetFristChild(string Name, bool isRecursive)
        {
            IComposite<T> composite;

            if (ChildDict.TryGetValue(FullName+ Separator+ Name, out composite))
                return composite;

            if (ChildDict == null)
                return null;

            if (isRecursive)
            {
                foreach (var com in ChildDict.Values)
                {
                    composite = com.GetFristChild(Name, true);
                    if (composite != null)
                        return composite;
                }
            }
            else
            {
                composite = null;
            }
            return composite;
        }

        public IComposite<T> GetParent()
        {
            return Parent;
        }

        public string GetFullName()
        {
            if (FullName != null)
                return FullName;
            else
                return "";
        }

        public IDictionary<string, IComposite<T>> GetAllChild()
        {
            if (ChildDict != null && ChildDict.Count > 0)
            {
                return ChildDict;
            }
            else
                return null;
        }

        public string GetOneLeafChild(Dictionary<string, IGetOneLeafChild<T>> Dict_Fun)
        {
            if (Dict_Fun != null&&Dict_Fun.Count>0)
            {
                var item = Dict_Fun[_funName_GetOneLeafChild];
                return item.GetOneLeafChild(_fullName);
            }
            else
                return "";
        }
    }   

}
