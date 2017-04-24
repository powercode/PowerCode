using System.Collections;
using System.Management.Automation;

namespace JoinItem {
    internal static class ObjectExtensions {
        public static object GetPropertyValue(this object o, string propertyName) {
            var pso = o as PSObject;
            if (pso != null)
                return pso.BaseObject?.GetPropertyValue(propertyName);
            switch (o) {
                case PSObject p:
                    return p.Properties[propertyName]?.Value;
                case Hashtable h:
                    return h.ContainsKey(propertyName) ? h[propertyName] : null;
                case IDictionary d:
                    return d.Contains(propertyName) ? d[propertyName] : null;

                default:
                    return PSObject.AsPSObject(o).Properties[propertyName]?.Value;
            }
        }
    }
}