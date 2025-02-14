# Architecture Rules

1. Layer Dependencies:
   - Cli -> Core <- Infrastructure
   - No circular dependencies
   - Core has no external dependencies

2. Domain Rules:
   - Keep domain models immutable
   - Use rich domain models over anemic ones
   - Validate domain invariants
   - Use value objects for complex values

3. Interface Design:
   - Interface segregation principle
   - Dependency inversion principle
   - Command Query Separation (CQS)
   - Favor composition over inheritance

4. Cross-Cutting Concerns:
   - Centralized error handling
   - Consistent logging patterns
   - Performance monitoring hooks
   - Proper resource disposal 