#!/bin/bash
# Build script for NoFirefightingInRain RimWorld mod


# Linux version
set -e

# Go to the folder where this script is located, then into src
cd "$(dirname "${BASH_SOURCE[0]}")/src"

dotnet build -c Release