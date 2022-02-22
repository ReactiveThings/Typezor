[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=ReactiveThings_Typezor)](https://sonarcloud.io/summary/new_code?id=ReactiveThings_Typezor)

# Typezor
Typezor is Source Generator based on [Typewriter](http://frhagn.github.io/Typewriter) that generates TypeScript files from c# code files using TypeScript Templates.

## Restrictions
- DocComment doesn't work for types from external libraries. It is known source generators and analyzers issue: https://github.com/dotnet/roslyn/issues/23673
