using System;

namespace Inventory.Items.Flags
{
    [Flags]
    public enum ToolCapability
    {
        None   = 0,
        Hand   = 1 << 0,
        Till   = 1 << 1,
        Loosen = 1 << 2,
        Water  = 1 << 3,
        Chop   = 1 << 4,
    }
}