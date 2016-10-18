using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Domain.Entities.Asset
{
    /// <summary>
    /// A shortcut of <see cref="AssetEntity{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    [Serializable]
    public abstract class AssetEntity : AssetEntity<int>
    {
    }
}
