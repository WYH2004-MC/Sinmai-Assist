#!/bin/sh
set -eu

script_dir="$(CDPATH= cd -- "$(dirname "$0")" && pwd)"
target_path="${1:-}"

if command -v git >/dev/null 2>&1; then
    commit_hash="$(git -C "$script_dir" rev-parse --short HEAD 2>/dev/null || printf '%s' 'NOT SET')"
else
    printf '%s\n' 'Git is not installed. Using default commit hash.'
    commit_hash="NOT SET"
fi

timestamp="$(date +"%Y%m%d%H%M%S")"

printf 'CommitHash="%s"\n' "$commit_hash"
printf 'BuildDate="%s"\n' "$timestamp"