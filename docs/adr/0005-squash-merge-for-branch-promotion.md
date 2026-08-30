# 5. Use squash merge for the automated branch promotion pipeline

## Status
Accepted

## Context

A four-tier environment branch strategy was established
(`sit → qa → prerelease → master`), with an automated promotion pipeline: a
push to `sit`, `qa`, or `prerelease` triggers a GitHub Actions workflow that
automatically opens a pull request promoting that branch's content into the
next stage. Each promotion PR still requires a manual review/merge click —
promotion is auto-*created*, not auto-*merged*.

GitHub's default pull request merge strategy is **"Merge commit"**, which
creates a new merge commit on the target branch for every PR merged, while
preserving every individual commit from the source branch.

In practice, this meant a single feature merged into `sit` produced **three
separate merge commits** by the time it reached `master` — one per promotion
hop (`sit → qa`, `qa → prerelease`, `prerelease → master`) — in addition to
the original feature-branch merge commit that landed it on `sit` in the first
place. `master`'s history became a long chain of near-identical "Merge pull
request #N" commits carrying no unique information, since each promotion PR's
diff is, by design, identical to the one before it — just relayed one stage
further.

This is meaningfully different from a typical team workflow (e.g. one used at
a previous employer), where each stage's PR represents a genuinely distinct,
separately reviewed event — a QA engineer testing on `qa`, a separate reviewer
approving before `prerelease`, etc. In that context, preserving a merge commit
per stage is justified, since each merge really did correspond to a distinct,
meaningful event. In this project, with a single solo developer and no
distinct per-stage review process (at least at this stage of the project),
every promotion hop is a purely mechanical relay of the same, already-reviewed
content — the meaningful review event already happened once, at the
feature-branch → `sit` merge.

## Decision

Configure the repository to allow **only "Squash and merge"** as the merge
strategy (Settings → General → Pull Requests: "Allow merge commits" and "Allow
rebase merging" both disabled, "Allow squash merging" left enabled), applying
uniformly to all pull requests in the repository, including both feature
branch → `sit` PRs and the three automated promotion PRs.

The general principle adopted: **use a real merge commit where the merge
represents a genuinely meaningful, distinct event in the project's history
(e.g. a feature landing for the first time); use squash where the merge is a
purely mechanical propagation of already-reviewed content (e.g. environment
promotion).** For this project's current setup — solo developer, no
distinct per-stage review — every PR in the repository currently falls into
the "propagation" category in effect, since feature work itself typically
lands as a single logical unit as well. Repo-wide squash-only was chosen over
a more precise (but more fragile) approach of manually selecting the correct
strategy per PR type, since the latter depends on remembering to choose
correctly every time.

## Consequences

**Positive:**
- `master`, `prerelease`, and `qa` histories stay clean — each promotion
  results in exactly one commit on the target branch, clearly labeled
  ("Promote sit to qa," etc.), rather than a chain of near-duplicate merge
  commits.
- Removes a class of manual error entirely — there's no merge-strategy
  dropdown decision to get wrong, since only one option exists.
- Simpler `git log` output across all branches, easier to reason about at a
  glance.

**Negative / trade-offs accepted:**
- If a feature branch itself contains several individually meaningful commits
  (e.g. distinct, well-described incremental steps), squashing collapses them
  into a single commit message on `sit`, losing that granularity. This is a
  real trade-off, not a limitation-free choice — accepted because, in
  practice, feature work in this project has generally landed as a single
  logical unit, and the loss is not currently costly.
- `git bisect` across a squashed history can only isolate a regression down to
  a whole squashed commit, not the specific smaller commit within it that
  caused the problem. Acceptable at current project scale.
- If this project moves to a team setting with genuinely distinct per-stage
  review, this decision should be revisited — real merge commits become
  valuable again once each stage's merge represents a distinct reviewed event
  worth preserving in history.

**Historical note:**
One batch of pre-existing merge-commit noise (PRs #14–#17, created before this
decision was made) was cleaned up via a **deliberate, temporary, one-time**
disabling of branch protection (to allow a force-push realigning `qa`,
`prerelease`, and `master` to `sit`'s cleaner history), followed immediately by
re-enabling protection and verifying it was active again. This was treated as
an explicit, conscious exception for cosmetic history cleanup — not a
precedent for routinely bypassing branch protection.

**Follow-up:**
- Revisit this decision if/when the project introduces genuine per-stage
  review (e.g. once collaborators join, or once deployment targets exist and
  each stage represents real, distinct verification).
