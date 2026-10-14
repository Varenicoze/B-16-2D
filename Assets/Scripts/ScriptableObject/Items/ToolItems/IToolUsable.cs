using Inventory.Items.Flags;
using UnityEngine;

namespace Inventory.Items.ToolItems
{
    public interface IToolUsable
    { 
        ToolCapability Capability { get; }
        int Volume { get; }
    }
}