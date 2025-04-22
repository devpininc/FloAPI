User Management – FloAPI (Summary)

- Use a single MongoDB collection: `Flo_Users`
- Store all users (`SysAdmin`, `Admin`, `Agent`, `Client`) with a `Role` field
- No passwords — **magic link only** login via email
- Once logged in, issue:
  - JWT access token (15–60 min)
  - Refresh token (7–30 days)
- Store magic token fields in `Flo_Users`: `MagicToken`, `MagicTokenExpiresAt`, `MagicTokenUsed`
- Keep users logged in using refresh tokens (mobile/web safe)
- All fields like `RecoNumber`, `TradeName`, `TrialExpiresAt` live in `Flo_Users`
- Collection name: `Flo_Users` (prefix aligned with FloAPI)
- Database: `homepin`
- Roles enforced using `[Authorize(Roles = "...")]`
