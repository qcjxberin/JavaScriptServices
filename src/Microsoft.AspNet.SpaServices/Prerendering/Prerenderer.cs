using System;
using System.Threading.Tasks;
using Microsoft.AspNet.NodeServices;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNet.SpaServices.Prerendering
{
    public static class Prerenderer
    {
        private static Lazy<StringAsTempFile> nodeScript;
        
        static Prerenderer() {
            nodeScript = new Lazy<StringAsTempFile>(() => {
                var script = EmbeddedResourceReader.Read(typeof(Prerenderer), "/Content/Node/prerenderer.js");
                return new StringAsTempFile(script); // Will be cleaned up on process exit
            });
        }
        
        public static async Task<RenderToStringResult> RenderToString(INodeServices nodeServices, string componentModuleName, string componentExportName, string requestAbsoluteUrl, string requestPathAndQuery) {
            return await nodeServices.InvokeExport<RenderToStringResult>(nodeScript.Value.FileName, "renderToString", 
                /* bootModulePath */ componentModuleName,
                /* bootModuleExport */ componentExportName,
                /* absoluteRequestUrl */ requestAbsoluteUrl,
                /* requestPathAndQuery */ requestPathAndQuery);
        }
    }
    
    public class RenderToStringResult {
        public string Html;
        public JObject Globals;
    }
}