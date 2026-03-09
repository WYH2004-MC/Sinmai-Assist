#!/bin/sh
set -eu

script_dir="$(CDPATH= cd -- "$(dirname "$0")" && pwd)"

printf '%s\n' 'Before Building Action...'

if command -v git >/dev/null 2>&1; then
    commit_hash="$(git -C "$script_dir" rev-parse --short HEAD 2>/dev/null || printf '%s' 'NOT SET')"
else
    printf '%s\n' 'Git is not installed. Using default commit hash.'
    commit_hash="NOT SET"
fi

timestamp="$(date -u +"%Y-%m-%dT%H:%M:%SZ")"

printf 'CommitHash="%s"\n' "$commit_hash"
printf 'BuildDate="%s"\n' "$timestamp"

cat > "$script_dir/BuildInfo.cs" <<EOC
namespace SinmaiAssist {
    public static partial class BuildInfo {
        public const string CommitHash = "$commit_hash";
        public const string BuildDate = "$timestamp";
    }
}
EOC
