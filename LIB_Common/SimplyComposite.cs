using System.Collections.Generic;

namespace LIB_Common
{
    delegate void DlgVerify<T>(object sender,T PNode,out bool result);
    //public interface ISimplyCompositeNode<T> where T : ISimplyCompositeNode<T>
    public interface ISimplyCompositeNode<T>
    {
        T                                               Node { get; set; }
        string                                          NodeID { get; set; }
        bool                                            AddChild(ISimplyCompositeNode<T> ChildNode);
        bool                                            ChildRemove(ISimplyCompositeNode<T> ChildNode);
        ISimplyCompositeNode<T>                         ParentNode { get; set; }
        IDictionary<string, ISimplyCompositeNode<T>>    AllChildNodesDictionary { get; }
        List<ISimplyCompositeNode<T>>                   ChildNodes { get; set; }



    }

    /// <summary>
    /// 简单复合模式结构(容器)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimplyComposite<T> : ISimplyCompositeNode<T>
    {
        private T _node;
        private string _nodeID;
        private ISimplyCompositeNode<T> _parent;
        private List<ISimplyCompositeNode<T>> _childs;
        private Dictionary<string, ISimplyCompositeNode<T>> _AllNodeDict;

        public SimplyComposite(T Node)
        {
            _node = Node;
        }
        #region ISimplyCompositeNode<T> Members

        T ISimplyCompositeNode<T>.Node
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

        string ISimplyCompositeNode<T>.NodeID
        {
            get
            {
                return _nodeID;
            }
            set
            {
                _nodeID = value;
            }
        }

        bool ISimplyCompositeNode<T>.AddChild(ISimplyCompositeNode<T> ChildNode)
        {
            var result = false;

            if (_childs == null)
                _childs = new List<ISimplyCompositeNode<T>>();

            _childs.Add(ChildNode);
            _AllNodeDict.Add(ChildNode.NodeID,ChildNode);
            ChildNode.ParentNode = this;

            return result;
        }

        bool ISimplyCompositeNode<T>.ChildRemove(ISimplyCompositeNode<T> ChildNode)
        {
            var result = false;

            if (_childs == null)
                _childs = new List<ISimplyCompositeNode<T>>();

            if (_childs.Contains(ChildNode))
            {
                result = _AllNodeDict.Remove(ChildNode.NodeID);

                return result && _childs.Remove(ChildNode);
            }
            else
            {
                return false;
            }
        }

        ISimplyCompositeNode<T> ISimplyCompositeNode<T>.ParentNode
        {
            get { return _parent; }
            set { _parent = value; }
        }


        #endregion

        #region ISimplyCompositeNode<T> Members


        IDictionary<string, ISimplyCompositeNode<T>> ISimplyCompositeNode<T>.AllChildNodesDictionary
        {
            get { return _AllNodeDict; }
        }

        List<ISimplyCompositeNode<T>> ISimplyCompositeNode<T>.ChildNodes
        {
            get { return _childs; }
            set { _childs = value; }
        }

        #endregion
    }
}
