namespace Fanex.CRM.Web.View.AppStart
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    public static class BundleExtensions
    {
        private static readonly CssRewriteUrlTransformWrapper _cssRewriteUrlTransformWrapper = new CssRewriteUrlTransformWrapper();

        public static void AddAndInclude(this BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
        {
            var bundle = new Bundle(bundleVirtualPath)
            {
                Transforms = { new StyleTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            };
            foreach (var path in virtualPaths)
            {
                bundle.Include(path, _cssRewriteUrlTransformWrapper);
            }
            bundles.Add(bundle);
        }
    }
}