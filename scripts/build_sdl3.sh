#!/usr/bin/env bash
set -e

PLATFORM="${GHA_OS_NAME}"
ARCH="${GHA_CMAKE_ARCH}"
ANDROID_API="${GHA_ANDROID_API}"
ANDROID_NDK="${GHA_ANDROID_NDK}"
ANDROID_CMAKE_FLAGS="${GHA_ANDROID_CMAKE_FLAGS}"

cd src
git clone --recursive https://github.com/libsdl-org/SDL.git SDL
cd SDL
git checkout release-3.2.10

CMAKE_ARGS="-DSDL_STATIC=OFF -DSDL_SHARED=ON -DSDL_TEST=OFF -DCMAKE_BUILD_TYPE=Release"

if [[ "$PLATFORM" == "android" ]]; then
    cmake -S . -B ./build $ANDROID_CMAKE_FLAGS -DCMAKE_ANDROID_ARCH_ABI=$ARCH -DCMAKE_ANDROID_API=$ANDROID_API $CMAKE_ARGS
elif [[ "$PLATFORM" == "linux" ]]; then
    cmake -S . -B ./build $CMAKE_ARGS
elif [[ "$PLATFORM" == "macos" ]]; then
    cmake -S . -B ./build -DCMAKE_OSX_ARCHITECTURES="$ARCH" $CMAKE_ARGS
elif [[ "$PLATFORM" == "windows" ]]; then
    cmake -S . -B ./build -A "$ARCH" $CMAKE_ARGS -DCMAKE_SYSTEM_VERSION=10.0.26100.0
else
    echo "Unknown platform: $PLATFORM"
    exit 1
fi

cmake --build ./build --config Release
cmake --install ./build --config Release --prefix install
