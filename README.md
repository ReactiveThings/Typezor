[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=ReactiveThings_Typezor)](https://sonarcloud.io/summary/new_code?id=ReactiveThings_Typezor)

# Typezor
Typezor is Source Generator based on [Typewriter](http://frhagn.github.io/Typewriter) that generates TypeScript files from c# code files using TypeScript Templates.

## Restrictions
- DocComment doesn't work for types from external libraries. It is known source generators and analyzers issue: https://github.com/dotnet/roslyn/issues/23673

## Features
- all leading and trailing whitespaces and new lines are removed from generated text
- if newly generated file has the same size as existing one then it is not overwritten ( This is performance optimization. In the edge case can cause outdated file content )
- Typezor returns all classes, interfaces and enums from compilation. For big project it can be very slow. You can quickly filter them by namespace using 
``Model.GetTypesFromNamespace("your.namespace","your.second.namespace")``
- Generated text can be saved as file ``@Output.SaveAs("path/to/file.ts")`` or added as source file to compilation ``@Output.AddSource("fileName")``
- Razor template file (.cshtml or .razor) must be added as additional file with attribute Typerazor="true" 
  ```
  <AdditionalFiles Include="templates\template.cshtml" Typezor="true" />
  ```
- Reusable code can be stored in separate .cs file 
  ```
  <AdditionalFiles Include="templates\shared.cs" Typezor="true" />
  ```
- External libraries used in template can be imported by adding dll file 
  ```
  <AdditionalFiles Include="libName.dll" Typezor="true" />
  ``` 
- External nuget packages can be used 
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
 <AdditionalFiles Include="templates\template.cshtml" IsRazorTemplate="true" Condition="'$(DesignTimeBuild)'!='true'"/>
 ```
