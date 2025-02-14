# C# Coding Rules

When writing C# code:

1. Use C# 12 features appropriately:
   - Required members
   - Primary constructors
   - Collection expressions
   - Alias any type
   - Inline arrays
   - Optional parameters in lambda expressions

2. Follow modern C# practices:
   - Use `init` properties for immutable objects
   - Utilize records for DTOs and value objects
   - Prefer pattern matching over type checking
   - Use switch expressions for concise branching
   - Implement IAsyncDisposable where appropriate

3. Code organization:
   - One class per file (except for small related classes)
   - Use partial classes for generated code
   - Keep methods focused and small
   - Use expression-bodied members when appropriate

4. Documentation:
   - XML comments on all public APIs
   - Include code examples in complex methods
   - Document threading/async behavior
   - Note any performance considerations 