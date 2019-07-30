namespace Fanex.CRM.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Models;

    public static class NameMapping
    {
        public static string[] GetCategoryContent(
            IEnumerable<SettingItem> settings,
            IEnumerable<Setting> mainCategories)
        {
            return GetCategoryContentWithoutSettingType(settings.Where(i => i.SettingType == "Category"), mainCategories);
        }

        public static string[] GetCategoryContentWithoutSettingType(
            IEnumerable<SettingItem> settings,
            IEnumerable<Setting> mainCategories)
        {
            return settings
                    .GroupBy(s => s.SettingIdLv1)
                    .Select(s =>
                    {
                        var con = GetMainCategoryName(s.Key, mainCategories);
                        var subIds = s.Select(e => e.SettingIdLv2).ToList();
                        foreach (var id in subIds)
                        {
                            con = string.Join(string.Empty, con, "\n  ", GetSubCategoryName(s.Key, id, mainCategories));
                        }

                        return con;
                    })
                    .ToArray();
        }

        public static string[] GetProductContent(
            IEnumerable<SettingItem> settings,
            IEnumerable<Product> productList,
            IEnumerable<Product> subProductList)
        {
            return settings
                  .Where(i => i.SettingType == "Product")
                  .GroupBy(s => s.SettingIdLv1)
                  .Select(s =>
                  {
                      var con = GetProductName(s.Key, productList);
                      var subIds = s.Select(e => e.SettingIdLv2).Distinct().ToList();
                      foreach (var id in subIds)
                      {
                          con = string.Join(string.Empty, con, "\n  ", GetSubProductName(id, subProductList));
                      }

                      return con;
                  })
                  .ToArray();
        }

        public static string[] GetCommunicationContent(
            IEnumerable<SettingItem> settings,
            IEnumerable<Setting> communicationToolList)
        {
            return settings
                   .Where(i => i.SettingType == "Communication")
                   .Select(s =>
                   {
                       return GetCommunicationName(s.SettingIdLv1, communicationToolList);
                   })
                   .ToArray();
        }

        public static string[] GetExtensionContent(
            IEnumerable<SettingItem> settings,
            IEnumerable<SettingRelation> extensionList)
        {
            return settings
                  .Where(i => i.SettingType == "Station")
                  .Select(s =>
                  {
                      return GetExtensionName(s.SettingIdLv1, extensionList);
                  })
                  .ToArray();
        }

        public static string GetMainCategoryName(int mainCategoryId, IEnumerable<Setting> mainCategories)
        {
            var mainCategory = mainCategories.FirstOrDefault(c => c.Id == mainCategoryId);
            return mainCategory == null ? string.Empty : mainCategory.Value;
        }

        public static string GetSubCategoryName(
            int mainCategoryId,
            int subCategoryId,
            IEnumerable<Setting> mainCategories)
        {
            var mainCategory = mainCategories.FirstOrDefault(c => c.Id == mainCategoryId);
            var child = mainCategory == null ? null : mainCategory.ChildSettings.FirstOrDefault(c => c.Id == subCategoryId);
            return child == null ? string.Empty : child.Name;
        }

        public static string GetProductName(int id, IEnumerable<Product> productList)
        {
            var product = productList.FirstOrDefault(c => c.Id == id);
            return product == null ? "N/A" : product.Name;
        }

        public static string GetSubProductName(int id, IEnumerable<Product> subProductList)
        {
            var subProduct = subProductList.FirstOrDefault(c => c.Id == id);
            return subProduct == null ? "N/A" : subProduct.Name;
        }

        public static string GetCommunicationName(int id, IEnumerable<Setting> communicationToolList)
        {
            var item = communicationToolList.FirstOrDefault(c => c.Id == id);
            return item == null ? string.Empty : item.Name;
        }

        public static string GetExtensionName(int id, IEnumerable<SettingRelation> extensionList)
        {
            var item = extensionList.FirstOrDefault(c => c.Id == id);
            return item == null ? "N/A" : item.Value;
        }
    }
}