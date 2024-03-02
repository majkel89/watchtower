# Watchtower

## Setup

```shell
dotnet tool restore
dotnet restore
```

## Requirements

 1. Scan common git repositories
    - GitLab
    - GitHub
    - Bitbucket
 2. Scan common technologies
    - dotnet
    - node.js
    - php
    - python
 3. Support monorepos
 4. Build dependency graph
    - list projects using given dependency
    - list dependency version usage
    - list projects using given dependencies by version expression
      - `<1.2.3`
      - `<1.2.3 >1.1.0`
      - `^1.2.3`
      - `1.*`
 5. Support CVS database
 6. List projects using vulnerable dependencies
 7. Support local vulnerabilities database
 8. Support internal and public dependencies
 9. Allow to rate dependencies
10. Allow to mark dependency as deprecated
11. Visualise dependencies
