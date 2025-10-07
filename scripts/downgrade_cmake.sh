#!/usr/bin/env bash
set -e

PLATFORM="${GHA_OS_NAME}"

if [[ "$PLATFORM" == "macos" ]]; then
    brew uninstall cmake
    wget https://github.com/Kitware/CMake/releases/download/v3.31.9/cmake-3.31.9-macos-universal.tar.gz

    tar -xzf cmake-3.31.9-macos-universal.tar.gz

    sudo mv cmake-3.31.9-macos-universal /usr/local/cmake-3.31.9
    echo "/usr/local/cmake-3.31.9/CMake.app/Contents/bin" >> $GITHUB_PATH
fi