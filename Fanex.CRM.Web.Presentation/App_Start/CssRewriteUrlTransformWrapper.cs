namespace Fanex.CRM.Web.View.AppStart
{
    using System.Web;
    using System.Web.Optimization;

    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        private readonly CssRewriteUrlTransform _cssTransform;

        public CssRewriteUrlTransformWrapper()
        {
            this._cssTransform = new CssRewriteUrlTransform();
        }

        public string Process(string includedVirtualPath, string input)
        {
            return this._cssTransform.Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}