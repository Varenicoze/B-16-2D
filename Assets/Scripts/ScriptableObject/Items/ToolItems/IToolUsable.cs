using Inventory.Item.Flags;
using UnityEngine;

namespace Inventory.Item.ToolItems
{
    public interface IToolUsable
    { 
        ToolCapability Capability { get; }
        int Volume { get; }
    }
}