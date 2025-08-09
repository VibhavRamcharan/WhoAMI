# Contributing to WhoAMI

First off, thank you for considering contributing to this project! Your help is greatly appreciated.

## Commit Message Guidelines

To ensure consistency and clarity in our commit history, we follow the [Conventional Commits](https://www.conventionalcommits.org/) specification. This format helps us automate versioning, generate changelogs, and makes the commit history easy to read.

### Format

The commit message should be structured as follows:

```
<type>[optional scope]: <short summary>

[optional body]

[optional footer(s)]
```

### Common Types

| Type     | Purpose                                                                          |
| :------- | :------------------------------------------------------------------------------- |
| `feat`   | A new feature                                                                    |
| `fix`    | A bug fix                                                                        |
| `docs`   | Documentation changes only                                                       |
| `style`  | Code style changes (formatting, missing semicolons, whitespace) — no code changes |
| `refactor`| Code changes that neither fix a bug nor add a feature                            |
| `perf`   | Code change that improves performance                                            |
| `test`   | Adding or updating tests                                                         |
| `build`  | Changes to build system or external dependencies (npm, Maven, Docker, etc.)      |
| `ci`     | Changes to CI/CD configuration (GitHub Actions, Jenkins, etc.)                   |
| `chore`  | Routine tasks, maintenance, or other changes that don’t modify src/test files    |
| `revert` | Revert a previous commit                                                         |

### Example Commit Messages

*   `feat(auth): add JWT token refresh endpoint`
*   `fix(api): correct null pointer error in user profile fetch`
*   `docs(readme): update installation instructions`
*   `style(ui): reformat header component with Prettier`
*   `refactor(db): extract connection logic into helper`
*   `perf(search): cache search results for faster response`
*   `test(auth): add unit tests for token expiration handling`

### Why We Use Conventional Commits

*   **Automation:** Helps tools like `semantic-release` automatically bump versions (`feat` → minor, `fix` → patch).
*   **Readability:** Makes it easier to scan the Git history and understand the changes.
*   **Changelogs:** Can be generated automatically from commit messages.
