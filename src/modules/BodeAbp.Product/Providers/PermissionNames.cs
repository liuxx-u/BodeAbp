using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodeAbp.Product.Providers
{
    internal class PermissionNames
    {
        //商品模块
        public const string Product = "Product";

        //分类
        public const string Product_Classify = "Product.Classify";
        public const string Product_Classify_Crud = "Product.Classify.Crud";

        //属性模版
        public const string Product_Attribute = "Product.Attribute";
        public const string Product_Attribute_Crud = "Product.Attribute.Crud";

        //属性值
        public const string Product_AttributeOption = "Product.AttributeOption";
        public const string Product_AttributeOption_Crud = "Product.AttributeOption.Crud";
    }
}
