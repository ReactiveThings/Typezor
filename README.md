[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=ReactiveThings_Typezor)](https://sonarcloud.io/summary/new_code?id=ReactiveThings_Typezor)

# Typezor
Typezor is Source Generator based on [Typewriter](http://frhagn.github.io/Typewriter) and [MiniRazor](https://github.com/Tyrrrz/MiniRazor) that generates TypeScript and C# files from C# source code using Razor Templates.

```razor
@namespace My.Namespace
@using System.Linq
@inherits Typezor.TemplateBase<Typezor.CodeModel.File>

@foreach (var c in Model.GetTypesFromNamespace("My.Namespace").Classes.Where(p => p.IsAbstract))
{
<text>
namespace My.Namespace.Implementations
{
    public class @(c.Name)Implementation : @c.Name
    {
    }
}
</text>
@Output.AddSource($"{c.Name}Implementation")
}
```

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <!-- Save generated C# source documents as files -->
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)\SourceGeneratorFiles</CompilerGeneratedFilesOutputPath>
    <UseRazorSourceGenerator>false</UseRazorSourceGenerator>
  </PropertyGroup>

  <ItemGroup>
    <!-- Add typezor source generator -->
    <PackageReference Include="Typezor.SourceGenerator" Version="0.3.1" PrivateAssets="analyzer" />
    <!-- Add typezor libraries to enable intelisense in template files -->
    <PackageReference Include="Typezor.Runtime" Version="0.3.1" PrivateAssets="all" />
    <PackageReference Include="Typezor.CodeModel" Version="0.3.1" PrivateAssets="all" />
      
    <!-- Include a single template -->
    <AdditionalFiles Include="templates/template.cshtml" Typezor="true" />

    <!-- Include multiple templates at once -->
    <AdditionalFiles Include="Templates/*.cshtml" IsRazorTemplate="true" />
    
    <!-- Disable code generation while typing -->
    <AdditionalFiles Include="templates/template.cshtml" IsRazorTemplate="true" Condition="'$(DesignTimeBuild)'!='true'"/>
    
    <!-- Include code from .cs file -->
    <AdditionalFiles Include="templates/shared.cs" Typezor="true" /> 

    <!-- Include library from dll library ( only .net Standard 2.0 libraries are supported ) -->
    <AdditionalFiles Include="libName.dll" Typezor="true" />
      
    <!-- Include library from nuget package ( only .net Standard 2.0 libraries are supported ) -->
    <PackageReference Include="Your.External.Lib.Name" Version="1.0.0" GeneratePathProperty="true" PrivateAssets="all" />
    <AdditionalFiles Include="$(PKGYour_External_Lib_Name)\lib\netstandard2.0\Your.External.Lib.Name.dll" Typezor="true" />

  </ItemGroup>

  <!-- ... -->
    <!-- Remove generated files during Clean -->
    <Target Name="CleanGeneratedFiles" AfterTargets="Clean">
        <ItemGroup>
            <GeneratedFiles Include="generated\*.*" />
        </ItemGroup>
        <Message Text="Deleting generated files" Importance="high" />
        <Delete Files="@(GeneratedFiles)" />
    </Target>

</Project>
```

## Restrictions
- DocComment returns empty string when build is performed using dotnet command. It is working only in Visual Studio.
- DocComment returns empty string for types from external libraries. It is known source generators and analyzers issue: https://github.com/dotnet/roslyn/issues/23673
- Requires .NET SDK 6.0.100 or later. Works also with SDK 7.0.304
- Include dll libraries doesn't work with sdk 6.0.200 because of bug in SDK [issue](https://developercommunity.visualstudio.com/t/NET-SDK-60200-breaks-build-due-to-CS2/1667603?ref=native&refTime=1645007418972&refUserId=91f26041-a17a-4035-a156-3704387e57e3) fixed in [PR](https://github.com/dotnet/roslyn/pull/59660)
- You can use it in Visual Studio 17.0.4 or later
- If you see an error after compilation "Error	CS8785	Generator 'RazorSourceGenerator' failed to generate source" you can fix it by adding UseRazorSourceGenerator with value false to .csproj file

```xml 
<PropertyGroup>
  <UseRazorSourceGenerator>false</UseRazorSourceGenerator>
</PropertyGroup>
```

## Features
- all leading and trailing whitespaces and new lines are removed from generated text
- if newly generated file has the same size as existing one then it is not overwritten ( This is performance optimization. In the edge case can cause outdated file content )
- Typezor returns all classes, interfaces and enums from compilation. For big project it can be very slow. You can quickly filter them by namespace using 
``Model.GetTypesFromNamespace("your.namespace","your.second.namespace")``
- Generated text can be saved as file ``@Output.SaveAs("path/to/file.ts")`` or added as source file to compilation ``@Output.AddSource("fileName")``
- Razor template file (.cshtml or .razor) must be added as additional file with attribute Typerazor="true" 
  ```
  <AdditionalFiles Include="templates/template.cshtml" Typezor="true" />
  ```
- Reusable code can be stored in separate .cs file 
  ```
  <AdditionalFiles Include="templates/shared.cs" Typezor="true" />
  ```
- External libraries used in template can be imported by adding dll file ( only .net Standard 2.0 libraries are supported )
  ```
  <AdditionalFiles Include="libName.dll" Typezor="true" />
  ``` 
- External nuget packages can be used ( only .net Standard 2.0 libraries are supported )
  ```
  <PackageReference Include="Your.External.Lib.Name" Version="1.0.0" GeneratePathProperty="true" PrivateAssets="all" />
  <AdditionalFiles Include="$(PKGYour_External_Lib_Name)\lib\netstandard2.0\Your.External.Lib.Name.dll" Typezor="true" />
  ```
- Performance logging can be enabled in .editorconfig file
  ```
  is_global = true 
  typezor_info_as_warning = true
  ```
- You can disable code generation during typing using DesignTimeBuild msbuild variable. Code will be generated only during build
 ```
 <AdditionalFiles Include="templates/template.cshtml" IsRazorTemplate="true" Condition="'$(DesignTimeBuild)'!='true'"/>
 ```
