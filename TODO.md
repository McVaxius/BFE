# BFE Maintainer TODO

## Verified baseline

- Pyros is the only active zone. It has a command and UI selection, a Kugane entry branch, a bunny FATE/combat loop, repair handling, and carrot-to-coffer routing.
- Pagos and Hydatos are disabled shells. Their UI choices are disabled, their command branches do not start automation, and their zone-entry and in-zone scheduler branches are placeholders. Territory IDs and stats models alone do not make either zone supported.
- Keep Pagos and Hydatos disabled until each zone meets the completion gate below.

## Pagos and Hydatos completion map

Complete every item independently for **each** zone. Do not assume that Pyros names, coordinates, targets, rewards, or route geometry apply elsewhere.

- [ ] Implement and manually verify zone entry from Kugane, including the correct selection callback, confirmation flow, territory transition, player-ready wait, and navmesh-ready wait.
- [ ] Identify every bunny FATE variant intended for support and define its reliable detection and wait-area boundary.
- [ ] Identify and verify the FATE combat targets, including the completion/boss target used to decide when combat is finished.
- [ ] Replace the zero repair-position placeholder with a manually verified mender location and verify both NPC repair and self-repair branches.
- [ ] Map every initial carrot direction message that can occur in the zone.
- [ ] Map every required checkpoint and the direction messages that can occur after each checkpoint.
- [ ] Record and manually verify every coffer coordinate reachable from every direction branch; leave no unmatched direction to a catch-all route unless that route is proven complete.
- [ ] Record and verify terrain-specific movement such as cliffs, jumps, drops, detours, and mount/dismount transitions.
- [ ] Implement the complete in-zone loop: travel to the correct bunny FATE, wait, level-sync, fight valid targets, obtain the bunny status, search for and open the coffer, then return to the next FATE cycle.
- [ ] Verify zone-specific coffer gil amounts and reward item IDs, update lifetime and session stats, and display the correct zone and reward labels.
- [ ] Verify that timer completion and optional retainer processing leave the zone only at a safe point, complete the configured outside-zone work, and either re-enter the selected zone or stop as configured.
- [ ] Enable the zone in the UI only after the full flow is supported; saved or default unsupported selections must never expose an active Start action.
- [ ] Enable the zone command only after it can start the same validated flow as the UI, with the same dependency and supported-zone checks.

## Pyros maintenance blockers

- [ ] Manually validate route coverage end to end: every initial direction, every checkpoint direction, every coffer-coordinate leaf, and every special movement branch. Correct only routes disproved by that validation.
- [ ] Replace the empty-direction `InitialStart()` restarts in `PyrosMovementHandler` with a safe failure state. An unread or unsupported toast must stop the current treasure search without consuming another carrot or moving blindly, and must clearly report why it stopped.
- [ ] Preserve manually validated Pyros coordinates, tolerances, route ordering, mount/dismount behavior, jump/drop handling, and other special movement while addressing the gaps above; do not broadly normalize or redesign the routing data.

## User-facing correctness

- [ ] Prevent unsupported default and persisted selections from starting automation. `Config.zoneSelected` currently defaults to Pagos (`0`) even though Pagos has no runtime, and the main Start button can still enable the scheduler for that selection.
- [ ] Correct the Hydatos command spelling from `hydatps` to `hydatos`; keep it non-starting until Hydatos passes the completion gate.
- [ ] Correct zone stats labels and reward tracking. The Pagos panel currently labels `bulbMinion` as "Eldthurs Mount" and `hakutakuEye` as "Pyros Hairstyles," the Hydatos panel heading says "Pagos," and reward-item counting currently covers only the Pyros item IDs.
- [ ] Align `README.MD` and `BFE/BFE.json` with actual support. Until another zone passes the completion gate, describe Pyros as the only supported zone and remove the expired promise that all three zones will be added by a past date.

## Zone completion gate

Do not enable Pagos or Hydatos in the UI or commands until its complete entry-to-exit flow and full treasure-route coverage have been manually validated. Validation must cover every supported FATE variant, combat and bunny acquisition, every carrot-direction/checkpoint/coffer branch, terrain exceptions, repair behavior, stats/rewards, repeated looping, timer exit, and the optional retainer round trip.

## Scope boundaries

- No general cleanup, refactoring, or architectural redesign.
- No changes to the vendored AutoRetainer API.
- No speculative features, dependencies, configuration systems, releases, packaging, version changes, or DevHub work.
- Keep deferred BFE maintenance work in this file only; do not create additional notes, reports, scripts, backups, checklists, or TODO locations.
