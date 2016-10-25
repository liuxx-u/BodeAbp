using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodeAbp.Scaffolding.Models
{
    public enum euColumnType
    {
        /// <summary>
        /// int
        /// </summary>
        intCT = 0x1
        ,
        /// <summary>
        /// decimal
        /// </summary>
        decimalCT = 0x3
        ,
        /// <summary>
        /// decimal
        /// </summary>
        longCT = 0x5
        ,
        /// <summary>
        /// float
        /// </summary>
        floatCT = 0x7
        ,
        /// <summary>
        /// double
        /// </summary>
        doubleCT = 0x9
        ,
        /// <summary>
        /// string
        /// </summary>
        stringCT = 0x0
        ,
        /// <summary>
        /// DateTime
        /// </summary>
        datetimeCT = 0x2
        ,
        boolCT = 0x4
        ,
        guidCT = 0x10
        ,
        /// <summary>
        /// RelatedModel
        /// </summary>
        RelatedModel = 0x20
    }
}
