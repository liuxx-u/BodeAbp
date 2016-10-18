using Abp.Authorization;
using Abp.Localization;

namespace BodeAbp.Product.Providers
{
    class BodeAbpProductAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var product = context.GetPermissionOrNull(PermissionNames.Product);
            if (product == null)
            {
                product = context.CreatePermission(PermissionNames.Product, L(PermissionNames.Product));

                //分类
                var productClassifys = product.CreateChildPermission(PermissionNames.Product_Classify, L(PermissionNames.Product_Classify));
                productClassifys.CreateChildPermission(PermissionNames.Product_Classify_Crud, L(PermissionNames.Product_Classify_Crud));

                //属性模版
                var productAttributes = product.CreateChildPermission(PermissionNames.Product_Attribute, L(PermissionNames.Product_Attribute));
                productAttributes.CreateChildPermission(PermissionNames.Product_Attribute_Crud, L(PermissionNames.Product_Attribute_Crud));

                //属性值
                var productAttributeOptions = product.CreateChildPermission(PermissionNames.Product_AttributeOption, L(PermissionNames.Product_AttributeOption));
                productAttributeOptions.CreateChildPermission(PermissionNames.Product_AttributeOption_Crud, L(PermissionNames.Product_AttributeOption_Crud));
            }
        }
        
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BodeAbpProductConsts.LocalizationSourceName);
        }
    }
}
