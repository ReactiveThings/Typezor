
${
    using Typezor.Extensions.Types;
    using System.Text;
    using System.Text.RegularExpressions;
    //<loadFromRemoteSources enabled="true"/>  
    //<configuration>  
   //<runtime>  
      //<loadFromRemoteSources enabled="true"/>  
   //</runtime>  
//</configuration>  
//https://stackoverflow.com/questions/28339116/not-allowed-to-load-assembly-from-network-location
//C:\Users\skube\AppData\Local\Microsoft\VisualStudio\16.0_6da7243a\devenv.exe.config


    string FileName(Class c) {
        return c.Name + ".ts";
    }

    Template(Settings settings)
    {

    }


}$Classes(p => p.Name.StartsWith("Class") && p.Namespace.StartsWith("Typezor.Tests.SourceGenerator"))[

export interface $Name {
$Properties()[
    $name: any;]
}

@@SaveAsFile:$FileName@@][]