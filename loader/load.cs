using Pchp.Core.Utilities;
using System;
using System.Linq;

public class loader
{
    public static object load()
    {
        var a = AppDomain.CurrentDomain.Load("ClassLibrary1");
        Type t = a.GetType("MyDb");
        //AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName == "ClassLibrary1");
        return Activator.CreateInstance(t);//, ContextExtensions.CurrentContext, null, null, null, null);
    }
}

