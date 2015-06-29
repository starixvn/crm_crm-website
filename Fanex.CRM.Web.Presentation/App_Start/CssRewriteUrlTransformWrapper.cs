namespace Fanex.CRM.Web.View.AppStart
{
    using System.Web;
    using System.Web.Optimization;

    /// <summary>
    /// Modify URL for style sheet by using virtual directory.
    /// </summary>
    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        private CssRewriteUrlTransform cssTransform;

        public CssRewriteUrlTransformWrapper()
        {
            this.cssTransform = new CssRewriteUrlTransform();
        }

        /// <summary>
        /// Processes the specified included virtual path.
        /// </summary>
        /// <param name="includedVirtualPath">The included virtual path.</param>
        /// <param name="input">The input.</param>
        /// <returns>Url Transform Object.</returns>
        public string Process(string includedVirtualPath, string input)
        {
            return this.cssTransform.Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}