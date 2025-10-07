#!/usr/bin/env bash
set -e

PLATFORM="${GHA_OS_NAME}"

if [[ "$PLATFORM" == "macos" ]]; then
    brew uninstall cmake
    brew install cmake@3.31.9
    brew link --force --overwrite cmake@3.31.9
fi