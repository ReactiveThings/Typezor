[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=ReactiveThings_Typezor)](https://sonarcloud.io/summary/new_code?id=ReactiveThings_Typezor)

# Typezor
Typezor is Source Generator based on [Typewriter](http://frhagn.github.io/Typewriter) that generates TypeScript files from c# code files using TypeScript Templates.

## Restrictions
- DocComment doesn't work for types from external libraries. It is known source generators and analyzers issue: https://github.com/dotnet/roslyn/issues/23673

## Features
- all leading and trailing whitespaces and new lines are removed from generated text
- if newly generated file has the same size as existing one then it is not overwritten ( This is performance optimization. In the edge case can cause outdated file content )
- Typerazor returns all classes, interfaces and enums from compilation. For big project it can be very slow. You can quickly filter them by namespace using 
``Model.GetTypesFromNamespace("your.namespace","your.second.namespace")``
