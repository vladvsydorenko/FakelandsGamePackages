using System.Collections;
using System.Collections.Generic;

//namespace Xyz.Vasd.Fake { public class index {} }

namespace Xyz.Vasd.Fake
{
    public interface INode
    {
        INode GetParent();
        List<INode> GetChildren();

        void AddChild(INode child);
        void RemoveChild(INode child);
        void SetParent(INode parent);
    }

    public interface ILoader
    {
        bool Load();
    }

    public interface ISpawner : ILoader
    {
        bool Spawn();
    }
}